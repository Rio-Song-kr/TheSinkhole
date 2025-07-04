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

        private void Update()
        {
            if (isPlayerInRange)
                Debug.Log("플레이어가 제작소 범위 안에 있음");
            // 플레이어가 범위 내에 있고 E키를 누르면 UI 오픈
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E키 입력 감지, 제작소 UI 오픈 시도");
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
                resourcesPanelController.SetRecipeList(filtered); // ResultPanelController의 메서드 사용
                resourcesPanelController.gameObject.SetActive(true); // UI 활성화
            }
        }
    }
}
