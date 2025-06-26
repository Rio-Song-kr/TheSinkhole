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
            foreach (var ing in recipe.ingredients)
            {
                var go = Instantiate(ingredientSlotPrefab, ingredientContentParent);
                var slot = go.GetComponent<IngredientSlotUI>();
                if (slot != null)
                    slot.Set(ing.item, ing.count);
            }

            // ��ư �ؽ�Ʈ
            craftingButtonText.text = "Crafting";

            // ��ư �̺�Ʈ ����
            craftingButton.onClick.RemoveAllListeners();
            craftingButton.onClick.AddListener(() =>
            {
                craftingManager.TryCraftWithDelay(recipe);
            });
        }
    }
}
