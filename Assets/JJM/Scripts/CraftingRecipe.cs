using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEditor;
using UnityEngine;

namespace CraftingSystem
{
    //인벤토리 구현이 완료될 시 구현에 사용된 내용을 참고하여 빨간줄 쳐진곳들의 이름을 수정
    //스크립터블 오브젝트로 생성하여 관리할 수 있도록 함
    [CreateAssetMenu(fileName = "Recipe", menuName = "scriptable Object/Crafting Recipe", order = int.MaxValue)]
    public class CraftingRecipe : ScriptableObject
    {
        //CraftingItemInfo를 배열로 사용하여 아이템 제작에 필요한 아이템들을 정의
        [Header("제작에 필요한 재료 아이템들")]//제작에 필요한 재료아이템s
        [SerializeField] public CraftingItemInfo[] reqItems;

        //CraftingItemInfo를 사용하여 제작 시 지급되는 아이템의 정보를 정의
        [Header("제작 결과물 아이템")]//제작 완료시 나올 아이템
        [SerializeField] public CraftingItemInfo resultItem;

        [Header("제작에 걸리는 시간")]//제작에 걸리는 시간
        [SerializeField] public float craftingTime;

        [Header("아이콘을 표시할 이미지")]//아이콘을 표시할 이미지
        [SerializeField] public Sprite buttonSprite;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(CraftingRecipe))]
    public class CraftingRecipeEditor : Editor
    {
        CraftingRecipe recipe;

        void OnEnable()
        {
            recipe = (CraftingRecipe)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //버튼 생성
            if (GUILayout.Button("이름 자동 변경"))
            {
                //이름 변경
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(recipe), $"RECIPE__{recipe.resultItem.item.ID.ToStirng()}");
            }
        }
    }
#endif
    //재료 및 결과 아이템에 대한 정보
    //어떤 아이템 및 아이템의 개수를 나타냄
    [System.Serializable]
    public struct CraftingItemInfo
    {
        //[SerializeField] public Item item; //아이템 (Item 부분은 인벤토리와 아이템이 만들어지면 수정)
        //[SerializeField] public int amount; //아이템 개수
    }
}

