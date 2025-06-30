using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem {
    

    [CreateAssetMenu(fileName = "Recipe", menuName = "Crafting/Recipe", order = 1)]
    public class CraftingRecipe : ScriptableObject
    {
        [Header("필요 재료 목록")]
        public CraftingItemInfo[] ingredients;//필요재료 목록

        [Header("제작 결과물")]
        public CraftingItemInfo result;//제작 결과물

        [Header("제작 시간(초)")]
        public float craftingTime = 5f;//제작시간 기본5초

        [Header("아이콘(버튼 등)")]
        public Sprite icon; //제작결과물 아이콘

        [Header("레시피가 들어갈 제작소")]
        public CraftingStationType stationType; // 이 레시피가 사용 가능한 제작대 타입

        [System.Serializable]
        public struct CraftingItemInfo
        {
            [Tooltip("아이템 데이터 SO")]
            public ItemDataSO item; //ItemSO로 만든 재료아이템
            [Tooltip("필요/결과 수량")]
            public int count;//제작하는데 필요한 수량
        }
    }
}
