using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem
{
    public class CraftingStation : MonoBehaviour
    {
        //해당 제작소에서 제작이 가능한 레세피들을 등록시킵니다
        [Header("제작 스테이션에서 가능한 조합 레시피들")]
        [SerializeField] private CraftingRecipe[] mRecipes;

        //간단한 아이템은 높은 티어의 제작소를 사용하지 않아도 제작할 수 있습니다
        //글로벌 레시피를 사용할 수 있는 기능을 추가로 구현
        //[Header("글로벌 레시피를 사용하는가?")]
        //[SerializeField] private bool mUseGlobalRecipes = true;

        ////제작소 다이얼로그 창을 열었을 때 해당 제작소의 이름을 UI에 표시하기 위해 사용
        //[Header("다이얼로그 창의 타이틀 이름")]
        //[SerializeField] private string mTitle = "CRAFTING STATION";

        //제작 다이얼로그 창을 염
        //CraftingManager에게 호출하여 자신의 레시피와 글로벌레시피를 사용하는지 타이틀 이름을 함께 전달하여 창을 열도록 함
        public void TryOpenDialog()
        {
            //CraftingManager.Instance.TryOpenDialog(mRecipes,mUseGlobalRecipes,mTitle);
        }
    }
}

