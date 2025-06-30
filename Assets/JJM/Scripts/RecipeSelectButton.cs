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

        // 버튼에 레시피 정보와 패널 컨트롤러를 세팅
        public void Init(CraftingRecipe recipe, RecipePanelController panel)
        {
            this.recipe = recipe;
            this.panelController = panel;
            recipeNameText.text = recipe.result.item.name;

            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnClickSelect);
        }

        private void OnClickSelect()
        {
            panelController.SetRecipe(recipe);
        }
    }
}