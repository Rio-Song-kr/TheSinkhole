using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지
        private static bool m_isInteractionKeyPressed = false;

        private void Update()
        {
            // 플레이어가 범위 내에 있고 E키를 누르면 UI 오픈
            if (isPlayerInRange && m_isInteractionKeyPressed)
            {
                GameManager.Instance.SetCursorUnlock();
                OpenStationUI();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어가 제작소 범위에 들어옴");
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("플레이어가 제작소 범위에서 나감");
                isPlayerInRange = false;
            }
        }
        // UI 오픈
        public void OpenStationUI()
        {
            if (resourcesPanelController != null)
            {
                // 이 제작대 타입에 맞는 레시피만 전달
                var filtered = availableRecipes.FindAll(r => r.stationType == stationType);

                // //todo ItemRecipeManager에서 station id로 recipe 목록 읽어오기
                // var recipes = new List<CraftingRecipe>();
                //
                // foreach (int itemId in GameManager.Instance.Recipe.StationIdRecipeList[stationType])
                // {
                //     int recipeId = GameManager.Instance.Recipe.ItemIdToRecipeId[itemId];
                //     var recipeDatas = GameManager.Instance.Recipe.RecipeIdData[recipeId];
                //
                //     foreach (var recipeData in recipeDatas)
                //     {
                //         int resultId = GameManager.Instance.Recipe.ItemIdToRecipeId[recipeData.RecipeId];
                //         var resultItemEnName = GameManager.Instance.Item.ItemIdEnName[resultId];
                //
                //         var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
                //         recipe.craftingTime = 5f;
                //         recipe.icon = GameManager.Instance.Item.ItemEnDataSO[resultItemEnName].Icon;
                //
                //         //todo 기타 데이터 연결해야 함
                //
                //         recipes.Add(recipe);
                //     }
                // }

                //todo recipe 목록 -> CraftingRecipe 생성(List에 추가) 후 전달
                resourcesPanelController.SetRecipeList(filtered); // ResultPanelController의 메서드 사용
                resourcesPanelController.gameObject.SetActive(true); // UI 활성화
            }
        }

        public static void OnInteractionKeyPressed() => m_isInteractionKeyPressed = true;
        public static void OnInteractionKeyReleased() => m_isInteractionKeyPressed = false;
    }
}