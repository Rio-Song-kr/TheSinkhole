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
        public GameObject recipeButtonPrefab;// ������ ��ư ������

        [Header("��� ��ũ�Ѻ� Content")]
        public Transform ingredientContentParent; // Content ������Ʈ

        [Header("���� ������")]
        public GameObject ingredientSlotPrefab; // ��� �� ĭ ������

        [Header("����")]
        public CraftingManager craftingManager;

        private CraftingRecipe currentRecipe;

        // ������ ������ UI ����
        public void SetRecipe(CraftingRecipe recipe)
        {
            // 1. UI ǥ��
            gameObject.SetActive(true);

            currentRecipe = recipe;

            // ��� ������ ���� ǥ��
            resultItemImage.sprite = recipe.result.item.Icon;
            resultItemName.text = recipe.result.item.name;
            itemDescription.text = recipe.result.item.ItemText;

            // ��� �ؽ�Ʈ(����)
            ingredientText.text = "�ʿ� ���";

            // ���� ��� ���� ����
            foreach (Transform child in ingredientContentParent)
                Destroy(child.gameObject);

            // ��� ���� ����
            //foreach (var ing in recipe.ingredients)
            //{
            //    Debug.Log($"Instantiate IngredientPanel: {ing.item?.name}, {ing.count}");

            //    int owned = 0;
            //    // �κ��丮���� �ش� �������� ������ ���
            //    if (craftingManager.playerInventory != null &&
            //        craftingManager.playerInventory.DynamicInventorySystem != null)
            //    {
            //        if (craftingManager.playerInventory.DynamicInventorySystem.FindItemSlots(ing.item, out var slots))
            //        {
            //            foreach (var slot in slots)
            //                owned += slot.ItemCount;
            //        }
            //    }

            //    var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);
            //    var slotUI = go.GetComponent<IngredientSlotUI>();
            //    if (slotUI != null)
            //        slotUI.Set(ing.item, ing.count, owned);

            //}
            var inventory = craftingManager.playerInventory;
            var invSys = inventory.DynamicInventorySystem;

            bool allEnough = true;
            bool allExact = true;

            foreach (var ing in recipe.ingredients)
            {
                int owned = 0;
                if (invSys.FindItemSlots(ing.item, out var slots))
                {
                    foreach (var slot in slots)
                        owned += slot.ItemCount;
                }

                // ��� ���� UI ���� �� ���� ó��
                var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);
                var slotUI = go.GetComponent<IngredientSlotUI>();
                if (slotUI != null)
                {
                    // ���� == �ʿ丸 ���, �� ��(�۰ų� ������) ������
                    slotUI.Set(ing.item, ing.count, owned);
                }

                if (owned < ing.count)
                    allEnough = false;
                if (owned != ing.count)
                    allExact = false;
            }

            // ��� �������� �κ��丮�� �̹� �ִ��� Ȯ��
            bool hasResultItem = invSys.FindItemSlots(recipe.result.item, out var resultSlots) && resultSlots.Count > 0;
            // �κ��丮�� �� ������ �ִ��� Ȯ��
            bool hasEmptySlot = invSys.HasEmptySlot();

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


            // ��ư �ؽ�Ʈ
            craftingButtonText.text = "Crafting";

            // ��ư �̺�Ʈ ����
            craftingButton.onClick.RemoveAllListeners();
            craftingButton.onClick.AddListener(() =>
            {
                craftingManager.TryCraftWithDelay(recipe);
            });
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
        }


        public CraftingRecipe GetCurrentRecipe()
        {
            return currentRecipe;
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
