using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CraftingSystem
{ 
    public class CraftingSlot : MonoBehaviour
    {
        //현재 제작을 하고 있는지 확인하기 위한 전역 변수
        //제작을 중복으로 하지 않도록 하기위함
        private static bool misCrafting = false;
        public static bool IsCrafting
        {
            get 
            { 
                return misCrafting; 
            }  
        }

        //제작 아이템의 결과를 보여줄 슬롯
        [Header("제작 결과 아이템의 슬롯")]
        [SerializeField] private InventorySlot mResultItemSlot;

        //아이템 제가에 요구되는 재료 아이템들을 보여줄 때 요구되는 아이템의 종류가 많은경우 한 슬롯에 모두 표시할 수 없음
        //스크롤 뷰를 사용하여 스크롤을 하여 어떤 아이템이 필요한지 모두 확인할 수 있게함
        //아이템 슬롯을 인스턴할 때 위치시킬 스크롤뷰의 콘텐츠 트랜스폼을 이곳에 등록
        [Header("제작에 필요한 재료 아이템을 담는 슬라이드 콘텐츠 트랜스폼")]
        [SerializeField] private Transform mRecipeContentTransform;

        //제작 버튼 활성화/비활성화 하기 위해 사용
        [Header("제작 버튼")]
        [SerializeField] private Button mCraftingButton;

        //제작에 소요되는 시간 및 제작 중 남은 시간을 표시하기위한 라벨
        [Header("제작 시간 텍스트 라벨")]
        [SerializeField] private TextMeshProUGUI mCraftingTimeLabel;

        //제작 시 진행중인 진척도를 표시하기위한 이미지
        [Header("제작 진행도 이미지")]
        [SerializeField] private Image mCraftingProgressImage;

        //제작이 불가능할 때 슬롯 위에 이 오브젝트를 덮어 시각적으로 제작이 불가능 하다는것을 보여주기 위한 오브젝트
        [Header("비활성화 상태시 보여줄 이미지 오브젝트")]
        [SerializeField] private GameObject mDisableImageGo;

        /// <summary>
        /// 현재 해당 슬롯이 사용중인 레시피
        /// </summary>
        [HideInInspector] public CraftingRecipe CurrentRecipe;
        private Coroutine? mCoCraftItem; //제작 연출 및 시간 계산 코루틴

        //제작 다이얼로그 창을 닫아 제작을 더 이상 하지 않거나 제작중인 아이템을 취소할때 호출됨
        //현재 실행중인 코루틴 중단 misCrafting 을 false로 설정
        private void OnDisable()
        {
            if (mCoCraftItem is not null)
            {
                StopCoroutine(mCoCraftItem);
            }
            misCrafting = false;
        }

        //레시피 슬롯 초기화
        //레시피를 매개변수로 사용 레시피를 기반으로 이 슬롯 초기화
        //레시피에 필요한 아이템의 개수만큼 인벤토리 슬롯을 생성 해당 슬롯에 등록
        //인벤토리 슬롯이 이미 있으면 해당 슬롯 재사용
        public void Init(CraftingRecipe recipe)
        {
            CurrentRecipe = recipe;
            
            //슬롯 활성화
            gameObject.SetActive(true);

            //제작 결과 아이템을 슬롯에 등록
            mResultItemSlot.ClearSlot();
            InventoryMain.Instance.AcquireItem(recipe.resultItem.item, mResultItemSlot,recipe.resultItem.count);

            //UI 요소 초기화
            mCraftingTimeLabel.text = $"{recipe.craftingTime.ToString("F1")}s";
            mCraftingProgressImage.fillAmount = 1.0f;
            mCraftingButton.GetComponent<Image>().sprite = recipe.buttonSprite;

            //제작 레시피의 재료 아이템 슬롯이 부족하면 개수에 맞게 인스턴스
            for (int i = mRecipeContentTransform.childCount; i < recipe.reqItems.Length; i++)
            {
                Instantiate(mResultItemSlot,Vector3.zero, Quaternion.identity, mRecipeContentTransform);
            }

            //모든 재료 슬롯을 초기화
            for (int i = 0; i < mRecipeContentTransform.childCount; i++)
            {
                //슬롯 획득
                InventorySlot recipeSlot = mRecipeContentTransform.GetChild(i).GetComponent<InventorySlot>();

                //레시피의 재료 개수보다 작은 인덱스 번호일경우
                if (i < recipe.reqItems.Length)
                {
                    //재료 아이템을 슬롯에 등록
                    recipeSlot.ClearSlot();
                    InventoryMain.Instance.AcquireItem(recipe.reqItems[i].item, recipeSlot, recipe.reqItems[i].count);
                    recipeSlot.gameObject.SetActive(true);
                }
                else
                {
                    recipeSlot.gameObject.SetActive(false);
                }
            }
        }

        //해당 아이템을 제작할 수 있는지 토글
        public void ToggleSlotState(bool isCraftable)
        {
            mDisableImageGo.SetActive(!isCraftable);
            mCraftingButton.interactable = isCraftable;
        }

        /// <summary>
        /// 재료 아이템을 제거하고, 결과 아이템을 획득
        /// </summary>
        
        //아이템을 제작해 해당 아이템 제작 소요시간이 지나면 아이템을 교환
        //플레이어의 인벤토리에서 재료 아이템을 제거하고 제작결과아이템을 인벤토리에 지급
        private void RefreshItems()
        {
            InventorySlot mainInventoryslot = null;

            //재료 아이템 정보를 확인하여 메인 인벤토리의 아이템을 제거
            foreach(CraftingItemInfo info in CurrentRecipe.reqItems)
            {
                InventoryMain.Instance.HasItemInInventory(info.item.ID, out mainInventoryslot, info.count);
                mainInventoryslot.UpdateItemCount(-info.count);
            }

            //제작 후 결과 아이템을 인벤토리에 획득
            InventoryMain.Instance.AcquireItem(CurrentRecipe.resultItem.item,CurrentRecipe.resultItem.count);

            //아이템을 교환 후 슬롯들을 업데이트
            CraftingManager.Instance.RefreshAllslots();
        }

        //아이템을 제작하는 소요시간만큼 UI요소를 갱신하고 시간이 경과했다면 아이템 교환을 하기위한 코루틴
        private IEnumerator CoCraftItem()
        {
            misCrafting = true;

            // 사운드 재생
            SoundManager.Instance.PlaySound2d("Craft Sound" + SoundManager.Range(1, 2));

            float process = 0f;
            while(process < 1f)
            {
                process += Time.deltaTime / CurrentRecipe.craftingTime;
                mCraftingProgressImage.fillAmount = Mathf.Lerp(0f,1f,process);

                yield return null;
            }
            //아이템 획득
            RefreshItems();
            misCrafting = false;
        }
        #region Ui

        //버튼을 눌러 아이템 제작 시도
        //이미 아이템을 제작중이라면 리턴, 다이얼로그 박스를 이용하여 알림을 띄워줌
        public void BTN_Craft()
        {
            if(misCrafting)
            {
                DialogBox.DialogBoxController dialogBox = DialogBoxGenerator.Instance.CreateSimpleDialogBox("알림",$"이미 제작중입니다.", "확인",dialogBox.DialogBoxController.RESERVED_EVENT_CLOSE,null,160,100);
                dialogBox.modifyBottomLayoutPadding(50, 50);
                return;
            }

            if(mCoCraftItem is not null)
            {
                StopCoroutine(mCoCraftItem);
            }
            mCoCraftItem = StartCoroutine(CoCraftItem());
        }
        #endregion

    }
}
