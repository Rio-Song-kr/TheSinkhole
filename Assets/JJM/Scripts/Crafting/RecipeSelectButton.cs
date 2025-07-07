using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftingSystem
{
    public class RecipeSelectButton : MonoBehaviour
    {
        public TMP_Text recipeNameText;
        public Button selectButton;
        public Image m_itemImage;

        private CraftingRecipe recipe;
        private RecipePanelController panelController;

        // ��ư�� ������ ������ �г� ��Ʈ�ѷ��� ����
        public void Init(CraftingRecipe recipe, RecipePanelController panel)
        {
            this.recipe = recipe;
            panelController = panel;
            recipeNameText.text = recipe.result.item.ItemData.ItemName;
            m_itemImage.sprite = recipe.icon;

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClickSelect);
        }

        private void OnClickSelect()
        {
            panelController.SetRecipe(recipe);
        }

        private void OnDisable()
        {
            panelController.CloseRecipeUI();
        }
    }
}