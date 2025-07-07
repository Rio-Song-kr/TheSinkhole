using System.Collections.Generic;
using EPOOutline;
using UnityEngine;

namespace CraftingSystem
{
    public class CraftingStation : MonoBehaviour
    {
        // 이 제작대의 타입(가공/소모 등)
        [Header("이 제작대의 타입")]
        public CraftingStationType stationType;

        [Header("이 제작대에서 제작 가능한 레시피 목록")]
        // 이 제작대에서 제작 가능한 레시피 목록
        public List<CraftingRecipe> availableRecipes;

        //[Header(" 전체 레시피 목록")]
        //// 전체 레시피 목록(게임 내 모든 레시피를 참조할 때 사용, 필요시 ScriptableObject로 관리)
        //public List<CraftingRecipe> allRecipes;

        [Header(" 레시피 패널 컨트롤러(UI) 참조")]
        // 레시피 패널 컨트롤러(UI) 참조
        public ResultPanelController resourcesPanelController;

        private bool canInteraction = false; // 플레이어가 범위 내에 있는지
        private static bool m_isInteractionKeyPressed = false;
        private Outlinable m_outline;

        private void Awake()
        {
            m_outline = GetComponent<Outlinable>();
            m_outline.enabled = false;
        }

        // UI 오픈
        public void OpenStationUI()
        {
            if (resourcesPanelController != null)
            {
                // 이 제작대 타입에 맞는 레시피만 전달
                // var filtered = availableRecipes.FindAll(r => r.stationType == stationType);

                // //todo ItemRecipeManager에서 station id로 recipe 목록 읽어오기
                var recipes = new List<CraftingRecipe>();

                foreach (int recipeID in GameManager.Instance.Recipe.StationIdRecipeList[stationType])
                {
                    //# 해당 Recipe id에 포함된 모든 레시피 목록을 불러옴
                    var recipeDatas = GameManager.Instance.Recipe.RecipeIdData[recipeID];

                    var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();

                    var craftItem = new CraftingItemInfo();

                    int resultId = GameManager.Instance.Recipe.RecipeIdToItemId[recipeID];
                    var resultItemEnName = GameManager.Instance.Item.ItemIdEnName[resultId];
                    recipe.icon = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName].Icon;

                    //# result 연결
                    craftItem.item = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName];
                    craftItem.count = 1;
                    recipe.result = craftItem;
                    recipe.craftingTime = 5f;

                    recipe.ingredients = new List<CraftingItemInfo>();

                    //# 각 레시피 목록 데이터를 하나씩 순회하면서 Icon과 Result 연결
                    foreach (var ingredientData in recipeDatas)
                    {
                        var ingredient = new CraftingItemInfo();
                        int ingredientItemId = ingredientData.MaterialId;
                        var ingredientItemEnName = GameManager.Instance.Item.ItemIdEnName[ingredientItemId];

                        ingredient.item = GameManager.Instance.Item.ItemEnDataSO[ingredientItemEnName];
                        ingredient.count = ingredientData.MaterialQuantity;

                        recipe.ingredients.Add(ingredient);
                    }
                    recipes.Add(recipe);
                }

                //todo recipe 목록 -> CraftingRecipe 생성(List에 추가) 후 전달
                resourcesPanelController.SetRecipeList(recipes); // ResultPanelController의 메서드 사용
                resourcesPanelController.gameObject.SetActive(true); // UI 활성화
            }
        }

        public void SetOutline(bool isEnable) => m_outline.enabled = isEnable;
    }
}