using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // 제작을 관리하는 매니저 클래스
    public class CraftingManager : MonoBehaviour
    {
        public Inventory playerInventory;// 플레이어 인벤토리 연결

        // 즉시 제작(제작 시간 무시)
        public void TryCraft(CraftingRecipe recipe)
        {
            if (CraftingHelper.Craft(recipe, playerInventory))
            {
                // 성공 시 추가 연출
            }
            else
            {
                // 실패 시 안내
            }
        }

        // 제작 시간 적용 (코루틴)
        public void TryCraftWithDelay(CraftingRecipe recipe)
        {
            if (CraftingHelper.CanCraft(recipe, playerInventory))
                StartCoroutine(CraftCoroutine(recipe));// 코루틴으로 제작

            else
                Debug.Log("재료가 부족합니다.");
        }

        // 실제 제작 시간 연출 및 제작 실행
        private IEnumerator CraftCoroutine(CraftingRecipe recipe)
        {
            Debug.Log($"{recipe.result.item.name} 제작 중... ({recipe.craftingTime}초)");
            yield return new WaitForSeconds(recipe.craftingTime);// 제작 시간 대기

            CraftingHelper.Craft(recipe, playerInventory);// 제작 실행
        }
    }
}
