using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



namespace CraftingSystem
{
    public class CraftingManager : Singleton<CraftingManager>
    {
        //현재 제작 다이얼로그 창이 열려있ㄴ느지(제작 시스템을 사용 중인지) 확인하기 위한 변수
        private static bool mIsDialogActive = false;
        public static bool IsDialogActive
        {
            get
            {
                return mIsDialogActive;
            }
        }

        //제작소에서 사용할 수 있는 글로벌 레시피
        //제작소 별도의 레시피에서 이 글로벌 레시피를 포함할 수 있음
        [Header("Crafting Station에 상관 없이 전역으로 사용 가능한 레시피")]
        [SerializeField] private CraftingRecipe[] mGlobalRecipes;

        //전역 레시피를 사용하지 않을 때 해당 레시피들을 임시로 옮겨둘 트랜스폼
        [Header("전역 레시피를 사용하지 않을 때 슬롯들을 임시로 둘 트랜스폼")]
        [SerializeField] private Transform mGlobalRecipesTemporaryPlacement;

        //제작 다이얼로그 창을 활성화 및 비활성화 하기 위한 게임오브젝트
        [Header("Crafting 다이얼로그 창 오브젝트")]
        [SerializeField] private GameObject mCraftingDialogGo;

        //스크롤 뷰에서 제작 레시피 슬롯을 배치하기 위한 콘텐츠 트랜스폼
        [Header("Crafting 다이얼로그에 레시피를 배치할 Content 트랜스폼")]
        [SerializeField] private Transform mRecipeContentTransform;

        [Header("Crafting Slot 프리팹")]
        [SerializeField] private GameObject mRecipeSlotPrefab;

        //UI 요소들을 정의
        //제작 가능한 아이템만 볼 수 있도록 설정하는 토글과 다이얼로그 창의 타이틀 라벨
        [Space(30)][Header("UI 요소들")]
        [Header("제작 가능한 아이템만 보도록 하는 토글")]
        [SerializeField] private Toggle mViewCraftableOnlyToggle;
        [Header("다이얼로그 창 타이틀")]
        [SerializeField] private TextMeshProUGUI mTitleLabel;

        CraftingSlot[] mGlobalRecipeSlots; //글로벌 레시피 슬롯들
        List<CraftingSlot> mStationOnlyRecipeSlots = new List<CraftingSlot>(); //스테이션 고유 레시피 슬롯들

        private int mCurrentCraftingCount;//현재 제작 스테이션의 별도 레시피 개수

        private Inventory playerInventory;
        //private void Awake()
        //{
        //    //초기화시 전역 활성화 상태 해제
        //    CraftingManager.mIsDialogActive = false;

        //    var playerObj = GameObject.FindWithTag("Player");
        //    if (playerObj != null)
        //        playerInventory = playerObj.GetComponent<Inventory>();

        //    Init();
        //}

        /// <summary>   
        /// 전역 레시피를 초기화
        /// </summary>

        //씬이 로드되면 글로벌 레시피를 미리 로드하고 프리팹화하여 사용 준비를 완료
        //임시로 mGlobalRecipesTemporaryPlacement에 위치하여 글로벌 레시피를 사용할 때만 적절한 위치로 이동
        private void Init()
        {
            List<CraftingSlot> globalRecipeSlots = new List<CraftingSlot>();
            foreach (CraftingRecipe recipe in mGlobalRecipes)
            {
                //인스턴스 및 초기화
                CraftingSlot craftingSlot = Instantiate(mRecipeSlotPrefab, Vector3.zero, Quaternion.identity, mGlobalRecipesTemporaryPlacement).GetComponent<CraftingSlot>();
                //craftingSlot.Init(recipe, playerInventory);

                //리스트에 삽입
                globalRecipeSlots.Add(craftingSlot);
            }
            mGlobalRecipeSlots = globalRecipeSlots.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipes">다이얼로그에 포함시킬 레시피들</param>
        /// <param name="useGlobalRecipes">글로벌 레시피를 사용할지 여부</param>"

        //CraftingStation.cs 로부터 호출되어 매개변수로 레시피와 글로벌 레시피를 사용하는지 타이틀은 무엇인지 넘겨주며 초기화를 할수 있도록 함
        //부족한 레시피 슬롯 프리팹을 인스턴스화하여 레시피 개수에 맞게 만든 후 해당 슬롯 프리팹에 Init을 호출하여 레시피를 설정함
        public void TryOpenDialog(CraftingRecipe[] recipes, bool useGlobalRecipes, string title)
        {
            //이미 다이얼로그가 켜져있으면
            if (mIsDialogActive)
            {
                return;
            }

            //부족한 레시피 슬롯 오브젝트를 인스턴스 및 리스트에 관리
            for (int i = mStationOnlyRecipeSlots.Count; i < recipes.Length; i++)
            {
                CraftingSlot craftingSlot = Instantiate(mRecipeSlotPrefab, Vector3.zero, Quaternion.identity, mRecipeContentTransform).GetComponent<CraftingSlot>();
                mStationOnlyRecipeSlots.Add(craftingSlot);
            }

            //모든 슬롯을 검사하여 활성화 및 비활성화
            for (int i = 0; i < mStationOnlyRecipeSlots.Count; i++)
            {
                //레시피의 개수보다 작은 인덱스 번호일경우
                if (i < recipes.Length)
                {
                    mStationOnlyRecipeSlots[i].Init(recipes[i], playerInventory);
                }
                else
                {
                    mStationOnlyRecipeSlots[i].gameObject.SetActive(false);
                }
            }

            //글로벌 레시피 사용 유무 설정
            if (useGlobalRecipes)
            {
                foreach (CraftingSlot globalRecipe in mGlobalRecipeSlots)
                {
                    globalRecipe.transform.SetParent(mRecipeContentTransform);
                }
            }
            //다이얼로그 박스활성화
            mCraftingDialogGo.gameObject.SetActive(true);

            mTitleLabel.text = title;
            mCurrentCraftingCount = recipes.Length;
            mIsDialogActive = true;
            //UtilityManager.UnlockCursor();

            //모든 슬롯을 갱신
            RefreshAllSlots();
        }
        /// <summary>
        /// 다이얼로그를 닫음
        /// </summary>

        //다이얼로그 창을 비활성화
        public void CloseDialog()
        {
            // 글로벌 레시피를 모두 옮김
            foreach (CraftingSlot globalRecipe in mGlobalRecipeSlots)
            {
                globalRecipe.transform.SetParent(mGlobalRecipesTemporaryPlacement);
            }
            //다이얼로그 비활성화
            mCraftingDialogGo.gameObject.SetActive(false);

            mIsDialogActive = false;
            //UtilityManager.TryLockCursor();
        }

        /// <summary>
        /// 해당 슬롯을 이용 가능한지 검사하여 슬롯에 상태를 적용
        /// </summary>
        /// <param name="craftingslot"></param>

        //해당 슬롯이 현재 제작이 간으한지 검사하여 검사 후 제작 여부를 토글
        private void CheckCraftingSlot(CraftingSlot craftingSlot)
        {
            //슬롯을 활성화
            craftingSlot.gameObject.SetActive(true);

            var inventorySystem = playerInventory.InventorySystem;

            //요구 아이템이 플레이어의 인벤토리에 있는지 검사
            for (int i = 0; i < craftingSlot.CurrentRecipe.reqItems.Length; i++)
            {
                //하나라도 아이템 재료가 없다면 비활성화 상태로 전환
                //if (InventoryMain.Instance.HasItemInInventory(craftingSlot.CurrentRecipe.reqItems[i].item.ID, craftingSlot.CurrentRecipe.reqItems[i].count) == false)
                var reqInfo = craftingSlot.CurrentRecipe.reqItems[i];
                int requiredCount = reqInfo.count;
                int ownedCount = 0;

                if (inventorySystem.FindItemSlots(reqInfo.item, out var slots))
                {
                    foreach (var slot in slots)
                        ownedCount += slot.ItemCount;
                }

                //if (ownedCount < requiredCount)
                //{
                //    //제작이 불간으한 상태에서 ViewCraftableOnly가 켜져있다면
                //    if (mViewCraftableOnlyToggle.isOn)
                //    {
                //        craftingSlot.gameObject.SetActive(false);//오브젝트 자체를 비활성화
                //    }
                //    else
                //    {
                //        craftingSlot.ToggleSlotState(false);//제작이 불가능한 상태로 보여지게함
                //    }
                //    return;
                //}
            }
            //요구 아이템이 모두 있는경우 활성화 상태로 전화
            craftingSlot.ToggleSlotState(true);
        }
        /// <summary>
        /// 모든 슬롯을 갱신
        /// </summary>

        //특정 조건에 의해 모든 슬롯이 갱신이 될 필요가 있다면 호출하여 갱신
        //에를 들어 플레이어가 제작을 하여 소지 아이템이 바뀐경우 호출
        public void RefreshAllSlots()
        {
            //다이얼로그가 비활성화 상태라면
            if (mIsDialogActive == false)
            {
                return;
            }
            for (int i = 0; i < mCurrentCraftingCount; i++)
            {
                CheckCraftingSlot(mStationOnlyRecipeSlots[i]);
            }

            //mGlobalRecipesTemporaryPlacement의 자식 개수가 0
            if (mGlobalRecipesTemporaryPlacement.childCount == 0)
            {
                foreach (CraftingSlot globalRecipeSlot in mGlobalRecipeSlots)
                {
                    CheckCraftingSlot(globalRecipeSlot);
                }
            }
        }
        #region

        //토글에 의해 호출되며 제작 가능한 대상만 볼 것인지 선택
        public void TOGGLE_ViewCraftableOnly()
        {
            RefreshAllSlots();
        }
        #endregion
    }
}

