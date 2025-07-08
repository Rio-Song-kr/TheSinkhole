using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class RecipePanelController : MonoBehaviour
    {
        [Header("UI ����")]
        public Image resultItemImage;
        public TMP_Text resultItemName;
        // ��� ���� ���� �� ���� ����

        public TMP_Text itemDescription;
        public TMP_Text ingredientText;
        public Button craftingButton;
        public TMP_Text craftingButtonText;

        [Header("RecipeSelectButton ����")]
        public GameObject recipeButtonPrefab; // ������ ��ư ������

        [Header("��� ��ũ�Ѻ� Content")]
        public Transform ingredientContentParent; // Content ������Ʈ

        [Header("���� ������")]
        public GameObject ingredientSlotPrefab; // ��� �� ĭ ������

        [Header("����")]
        public CraftingManager craftingManager;

        private CraftingRecipe currentRecipe;
        private List<IngredientSlotUI> m_ingredientLists = new List<IngredientSlotUI>();

        public static bool NeedUpdateUI = false;

        private void Update()
        {
            if (NeedUpdateUI != true) return;

            NeedUpdateUI = false;

            var inventory = craftingManager.playerInventory;
            var ing = currentRecipe.ingredients;

            bool allEnough = true;
            bool allExact = true;

            for (int i = 0; i < ing.Count; i++)
            {
                allEnough = CheckEnough(ing[i], allEnough, ref allExact, i);
            }

            CanCrafting(currentRecipe, inventory, allEnough, allExact);
        }

        public void CloseRecipeUI() => gameObject.SetActive(false);

        // ������ ������ UI ����
        public void SetRecipe(CraftingRecipe recipe)
        {
            // 1. UI ǥ��
            gameObject.SetActive(true);

            currentRecipe = recipe;

            // ��� ������ ���� ǥ��
            resultItemImage.sprite = recipe.result.item.Icon;
            resultItemName.text = recipe.result.item.ItemData.ItemName;
            itemDescription.text = recipe.result.item.ItemText;

            // ��� �ؽ�Ʈ(����)
            ingredientText.text = "�ʿ� ���";

            // ���� ��� ���� ����
            foreach (Transform child in ingredientContentParent)
            {
                Destroy(child.gameObject);
            }

            bool allEnough = true;
            bool allExact = true;

            foreach (var ing in recipe.ingredients)
            {
                allEnough = CheckEnough(ing, allEnough, ref allExact);
            }

            CanCrafting(recipe, craftingManager.playerInventory, allEnough, allExact);


            // ��ư �ؽ�Ʈ
            craftingButtonText.text = "Crafting";

            // ��ư �̺�Ʈ ����
            craftingButton.onClick.RemoveAllListeners();
            craftingButton.onClick.AddListener(() => { craftingManager.TryCraftWithDelay(recipe); });
        }

        private void CanCrafting(CraftingRecipe recipe, Inventory inventory, bool allEnough, bool allExact)
        {
            // ��� �������� �κ��丮�� �̹� �ִ��� Ȯ��
            bool hasResultItem = inventory.GetItemAmounts(recipe.result.item.ItemEnName) > 0;

            // �κ��丮�� �� ������ �ִ��� Ȯ��
            bool hasEmptySlot = inventory.GetRemainingSlots() > 0;

            // ���� ���� ���� ����
            bool canCraft;
            if (hasEmptySlot || hasResultItem)
            {
                // ���� ����: ��Ḹ ����ϸ� ���� ����
                canCraft = allEnough;
            }
            else
            {
                // �� ���Ե� ����, ��� �����۵� ����
                // ��ᰡ "��Ȯ�� �ʿ䷮��ŭ" ���� ���� ���� ����
                canCraft = allExact;
            }
            craftingButton.interactable = canCraft && !craftingManager.IsCrafting;
        }

        private bool CheckEnough(CraftingItemInfo ing, bool allEnough, ref bool allExact, int index = -1)
        {
            var inventory = craftingManager.playerInventory;
            int owned = inventory.GetItemAmounts(ing.item.ItemEnName);

            // ��� ���� UI ���� �� ���� ó��
            IngredientSlotUI slotUI;
            if (index == -1)
            {
                var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);

                m_ingredientLists.Add(go.GetComponent<IngredientSlotUI>());

                slotUI = go.GetComponent<IngredientSlotUI>();
            }
            else
            {
                slotUI = m_ingredientLists[index];
            }
            if (slotUI != null)
            {
                // ���� == �ʿ丸 ���, �� ��(�۰ų� ������) ������
                slotUI.Set(ing.item, ing.count, owned);
            }

            if (owned < ing.count)
                allEnough = false;
            if (owned != ing.count)
                allExact = false;
            return allEnough;
        }

        public IEnumerator CraftingButtonCountdownCoroutine(float time)
        {
            string originalText = craftingButtonText.text; // ���� �ؽ�Ʈ ����

            int seconds = Mathf.CeilToInt(time);
            for (int i = seconds; i > 0; i--)
            {
                craftingButtonText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }
            craftingButtonText.text = "���ۿϷ�";
            yield return new WaitForSeconds(1f);

            craftingButtonText.text = originalText; // ���� �ؽ�Ʈ�� ����
            NeedUpdateUI = true;
        }

        public CraftingRecipe GetCurrentRecipe() => currentRecipe;
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}