using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{

    // 제작을 관리하는 매니저 클래스
    public class CraftingManager : MonoBehaviour
    {
        public Inventory playerInventory;// 플레이어 인벤토리 연결

        private bool isCrafting = false; // 현재 제작 중인지 여부

        public bool IsCrafting => isCrafting;

        private void Start()
        {
            // 자동 연결: "Player" 태그가 붙은 오브젝트에서 Inventory 컴포넌트 찾기
            if (playerInventory == null)
            {
                var playerObj = GameObject.FindWithTag("Player");
                if (playerObj != null)
                { 
                    playerInventory = playerObj.GetComponent<Inventory>();
                if (playerInventory != null)
                    Debug.Log("Player Inventory 자동 연결 성공");
                else
                    Debug.LogWarning("Player 오브젝트에 Inventory 컴포넌트가 없습니다.");
            }
            else
            {
                Debug.LogWarning("Player 태그 오브젝트를 찾을 수 없습니다.");
            }
        }
            else
            {
                Debug.Log("Player Inventory가 이미 Inspector에서 연결되어 있습니다.");
            }
        }

        // 즉시 제작(제작 시간 무시)
        public void TryCraft(CraftingRecipe recipe)
        {
            //즉시 제작 기능 넣을 시 작성
        }

        // 제작 시간 적용 (코루틴)
        public void TryCraftWithDelay(CraftingRecipe recipe)
        {
            if (isCrafting)
            {
                Debug.Log("제작 중입니다. 다른 제작은 불가능합니다.");
                return;
            }
            if (CraftingHelper.CanCraft(recipe, playerInventory))
                StartCoroutine(CraftCoroutine(recipe));// 코루틴으로 제작

            else
                Debug.Log("재료가 부족합니다.");
        }

        // 실제 제작 시간 연출 및 제작 실행
        private IEnumerator CraftCoroutine(CraftingRecipe recipe)
        {
            isCrafting = true; // 제작 시작
            Debug.Log($"{recipe.result.item.name} 제작 중... ({recipe.craftingTime}초)");
            yield return new WaitForSeconds(recipe.craftingTime);// 제작 시간 대기

            CraftingHelper.Craft(recipe, playerInventory);// 제작 실행
            isCrafting = false; // 제작 끝
        }
        
    }
}
