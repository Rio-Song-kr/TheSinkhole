using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // UI 버튼에서 제작을 실행하는 스크립트
    public class CraftingButton : MonoBehaviour
    {
        public RecipePanelController recipePanelController; // 패널 컨트롤러 참조

        public CraftingManager craftingManager;// 매니저 참조

        public void OnClickCraft()
        {
            var recipe = recipePanelController.GetCurrentRecipe();
            if (recipe != null)
                craftingManager.TryCraftWithDelay(recipe);
            else
                Debug.LogWarning("선택된 레시피가 없습니다!");

        }
    }
}
