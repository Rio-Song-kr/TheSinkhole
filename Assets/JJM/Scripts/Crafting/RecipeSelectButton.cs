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

        private CraftingRecipe recipe;
        private RecipePanelController panelController;

        // ��ư�� ������ ������ �г� ��Ʈ�ѷ��� ����
        public void Init(CraftingRecipe recipe, RecipePanelController panel)
        {
            this.recipe = recipe;
            panelController = panel;
            recipeNameText.text = recipe.result.item.name;

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