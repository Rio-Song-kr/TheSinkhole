using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public class RecipeListController : MonoBehaviour
    {
        public Transform contentParent; // 레시피 버튼들이 들어갈 Content
        public GameObject recipeButtonPrefab; // RecipeSelectButton 프리팹
        public RecipePanelController panelController; // 제작창 컨트롤러

        public CraftingRecipe[] recipes; // 전체 레시피 목록

        void Start()
        {
            foreach (var recipe in recipes)
            {
                var go = Instantiate(recipeButtonPrefab, contentParent);
                var btn = go.GetComponent<RecipeSelectButton>();
                btn.Init(recipe, panelController);
            }
        }
    }
}
