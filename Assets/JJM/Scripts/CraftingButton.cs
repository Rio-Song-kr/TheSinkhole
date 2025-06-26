using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    // UI 버튼에서 제작을 실행하는 스크립트
    public class CraftingButton : MonoBehaviour
    {
        public CraftingRecipe recipe;// 연결할 레시피
        public CraftingManager craftingManager;// 매니저 참조

        public void OnClickCraft()
        {
            craftingManager.TryCraftWithDelay(recipe);// 제작 시도(제작 시간 적용)

        }
    }
}
