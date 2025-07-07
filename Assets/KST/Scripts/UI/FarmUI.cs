using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class FarmUI : Singleton<FarmUI>
{
    [Header("UI")]
    public GameObject FarmUIGO;
    public bool GetActiveself() => FarmUIGO.activeSelf;

    public GameObject DetailGO;
    [SerializeField] private Image cropImg;
    [SerializeField] private TMP_Text cropName;
    [SerializeField] private TMP_Text cropDesc;
    [SerializeField] private TMP_Text cropRequireTime;
    [SerializeField] private TMP_Text m_statusText;
    public Image ProgressBarImg;

    [Header("ScrollView")]
    [SerializeField] private GameObject cropBtnPrefab;
    [SerializeField] private Transform scrollViewContentPos;//스크롤뷰 컨텐츠 위치
    public Button[] cropButtons;

    public CropDataSO[] CropList;

    [Header("Tile")]
    private FarmTile currentTile;
    private CropDataSO selectedCrop;

    private static bool m_isIneractionKeyPressed;
    private static bool m_isEscapeKeyPressed;

    public event Action<bool> OnIsUIOpen;

    private float pressTimer = 0f;
    private float pressDuration = 5f;
    // private bool isPressingE = false;

    //인벤토리
    [SerializeField] private Inventory playerInven;

    private void Start()
    {
        ScrollViewSetting();
        m_statusText.text = "";
        ProgressBarImg.fillAmount = 0f;
        FarmUIGO.SetActive(false);
    }

    private void Update()
    {
        if (currentTile == null) return;

        if (m_isEscapeKeyPressed && !m_isIneractionKeyPressed)
        {
            CloseUI();
            return;
        }
        if (!GameTimer.IsDay)
        {
            if (currentTile.IsPlanted())
            {
                PlantingProgress();
            }
            else if (pressTimer > 0f)
            {
                CancelPlanting();
            }

            if (m_isIneractionKeyPressed)
            {
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.CantPlant);
            }
            return;
        }

        if (currentTile.IsPlanted())
        {
            PlantingProgress();
        }
        else if (selectedCrop != null)
        {
            //재배 가능한 상태
            if (m_isIneractionKeyPressed)
            {
                if (!HasRequiredItems(selectedCrop))
                {
                    m_statusText.text = "재배에 필요한 재료가 부족합니다.";
                    return;
                }
                // isPressingE = true;
                pressTimer += Time.deltaTime;
                ProgressBarImg.fillAmount = pressTimer / pressDuration;
                m_statusText.text = "작물 재배 중. . .";

                if (pressTimer >= pressDuration)
                {
                    StartGrowing(selectedCrop);
                }
            }
            else if (pressTimer > 0f)
            {
                CancelPlanting();
            }
        }
    }

    private void PlantingProgress()
    {
        var crop = currentTile.GetGrownCrop();
        DisplayCropDetail(crop);

        float remain = currentTile.GetRemainingGrowTime();

        if (currentTile.IsGrowing())
        {
            m_statusText.text = $"재배중 {FormatingTime.FormatMinTime(remain)}";
            ProgressBarImg.color = ColorUtil.Hexcode("#8CB4EF", Color.blue);
            ProgressBarImg.fillAmount = 1f;
        }
        else
        {
            m_statusText.text = "재배 완료! 수확하려면 [E]키를 누르세요";
            ProgressBarImg.color = ColorUtil.Hexcode("#8CEF8F", Color.green);
            ProgressBarImg.fillAmount = 1f;

            if (m_isIneractionKeyPressed)
            {
                Harvest();
                Debug.Log("수확");
            }
        }
    }

    public void OpenUI(FarmTile tile)
    {
        if (!GameTimer.IsDay) return;

        currentTile = tile;
        selectedCrop = null;
        pressTimer = 0f;
        // isPressingE = false;

        foreach (var btn in cropButtons)
            btn.interactable = true;

        FarmUIGO.SetActive(true);
        GameManager.Instance.SetCursorUnlock();
        OnIsUIOpen?.Invoke(true);

        m_isIneractionKeyPressed = false;

        m_statusText.text = "작물을 선택해주세요.";
        ProgressBarImg.fillAmount = 0f;
        DetailGO.SetActive(false);
    }

    public void CloseUI()
    {
        currentTile = null;
        FarmUIGO.SetActive(false);
        OnIsUIOpen?.Invoke(false);
        GameManager.Instance.SetCursorLock();
        m_isEscapeKeyPressed = false;
    }


    public void SelectCrop(CropDataSO crop)
    {
        //재배 중일 경우 다른 작물 선택 불가하도록
        if (currentTile == null || currentTile.IsPlanted())
        {
            if (currentTile != null && currentTile.GetGrownCrop() != crop)
                return;
        }

        selectedCrop = crop;
        DisplayCropDetail(crop);
        m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요.";
        ProgressBarImg.fillAmount = 0f;
    }

    //재배 시작
    private void StartGrowing(CropDataSO crop)
    {
        if (!GameTimer.IsDay) return; //낮이 아니면 리턴

        if (!HasRequiredItems(crop))
        {
            m_statusText.text = "재료가 부족합니다.";
            return;
        }

        if (!ConsumeRequiredItems(crop))
        {
            Debug.LogWarning("아이템 소모 오류");
            return;
        }

        currentTile.StartPlanting(crop);
        pressTimer = 0f;
        // isPressingE = false;

        m_statusText.text = "재배 시작됨";
        ProgressBarImg.fillAmount = 1f;

        foreach (var btn in cropButtons)
            btn.interactable = false;
    }

    private void CancelPlanting()
    {
        //상태 전부 초기화
        pressTimer = 0f;
        // isPressingE = false;
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요.";
    }

    //작물 설명 display
    private void DisplayCropDetail(CropDataSO data)
    {
        if (data == null) return;
        DetailGO.SetActive(true);

        cropImg.sprite = data.cropImg;
        cropName.text = data.cropName;
        cropRequireTime.text = $"{data.growTime} Seconds";
        cropDesc.text = $" ● 설명{data.cropDesc} \n ● 효과: {data.cropEffect}";
    }

    public void ScrollViewSetting()
    {
        var btnList = new List<Button>();
        foreach (Transform child in scrollViewContentPos)
        {
            Destroy(child.gameObject);
        }
        foreach (var crop in CropList)
        {
            var go = Instantiate(cropBtnPrefab, scrollViewContentPos);
            var cropBtn = go.GetComponent<CropBtn>();
            cropBtn.Init(crop);
            btnList.Add(cropBtn.Btn);
        }
        cropButtons = btnList.ToArray();
    }

    private void Harvest()
    {
        if (!GameTimer.IsDay) return; //낮이 아니면

        if (!AddInventory()) return; // 인벤에 예외 발생시.

        currentTile.HarvestingCrop();
        ProgressBarImg.fillAmount = 0f;
        ProgressBarImg.color = ColorUtil.Hexcode("#865A5A", Color.red);

        m_statusText.text = "재배하기 [E]키를 5초 동안 눌러주세요";

        foreach (var btn in cropButtons)
            btn.interactable = true;

        selectedCrop = null;
        DetailGO.SetActive(false);
    }
    //재료 아이템 체크
    private bool HasRequiredItems(CropDataSO crop)
    {
        foreach (var req in crop.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            int currentCount = playerInven.GetItemAmounts(req.ItemName);
            if (currentCount < req.RequireCount) return false;
        }
        return true;
    }

    private bool ConsumeRequiredItems(CropDataSO crop)
    {
        foreach (var req in crop.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            bool result = playerInven.RemoveItemAmounts(req.ItemName, req.RequireCount);
            if (!result)
            {
                m_statusText.text = "아이템 소모 실패";
                return false;
            }
        }
        return true;
    }

    private bool AddInventory()
    {
        if (selectedCrop == null || selectedCrop.harvestItemSo == ItemEnName.None)
        {
            Debug.LogWarning("수확할 작물이 없습니다.");
            return false;
        }
        // var item = selectedCrop.harvestItemSo;
        if (!GameManager.Instance.Item.ItemEnDataSO.TryGetValue(selectedCrop.harvestItemSo, out var item))
        {
            return false;
        }

        //1. 슬롯 검사
        int requireSlots = 1; // CropDataSo의 자료형이 harvestItemSo가 아닌 RequireItemData[]으로 선언된다면 해당 배열 개수로 받아오면 됨.

        if (playerInven.GetRemainingSlots() < requireSlots)
        {
            m_statusText.text = "인벤토리에 슬롯이 부족합니다.";
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
            return false;
        }

        //2. 아이템 추가
        int remain = playerInven.AddItemSmart(item, selectedCrop.harvestItemAmounts);
        if (remain > 0)
        {
            m_statusText.text = "인벤토리에 공간이 부족합니다.";
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
            return false;
        }

        //3. 수확
        GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, item, selectedCrop.harvestItemAmounts);
        return true;
    }

    public static void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
    public static void OnCloseKeyPressed() => m_isEscapeKeyPressed = true;
}