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
    private bool isPressingE = false;

    //인벤토리
    [SerializeField] private Inventory playerInven;

    private void Start()
    {
        ScrollViewSetting();
        m_statusText.text = "";
        ProgressBarImg.fillAmount = 0f;
    }

    private void Update()
    {
        if (currentTile == null) return;

        if (m_isEscapeKeyPressed && !m_isIneractionKeyPressed)
        {
            CloseUI();
            return;
        }
        if (!GameTimer.IsDay) return;

        if (currentTile.IsPlanted())
        {
            var crop = currentTile.GetGrownCrop();
            DisplayCropDetail(crop);

            float remain = currentTile.GetRemainingGrowTime();

            if (currentTile.IsGrowing())
            {
                m_statusText.text = $"재배중 {FormatingTime.FormatMinTime(remain)}";
                ProgressBarImg.fillAmount = 1f;
            }
            else
            {
                m_statusText.text = "재배 완료! 수확하려면 [E]키를 누르세요";
                ProgressBarImg.fillAmount = 1f;

                if (m_isIneractionKeyPressed)
                {
                    Harvest();
                    Debug.Log("수확");
                }
            }
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
                isPressingE = true;
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

    public void OpenUI(FarmTile tile)
    {
        if (!GameTimer.IsDay) return;

        currentTile = tile;
        selectedCrop = null;
        pressTimer = 0f;
        isPressingE = false;

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

        ConsumeRequiredItems(crop);

        currentTile.StartPlanting(crop);
        pressTimer = 0f;
        isPressingE = false;

        m_statusText.text = "재배 시작됨";
        ProgressBarImg.fillAmount = 1f;

        foreach (var btn in cropButtons)
            btn.interactable = false;
    }

    private void CancelPlanting()
    {
        //상태 전부 초기화
        pressTimer = 0f;
        isPressingE = false;
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
        cropDesc.text = $"소요 시간 : {data.growTime}초 \n {data.cropDesc} \n 배고픔 : {data.cropEffect} %";
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
        if (!AddInvetory() || !GameTimer.IsDay) return; // 인벤에 자리 없거나, 낮이 아니면

        currentTile.HarvestingCrop();
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = "수확 완료!";

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
            int currentCount = playerInven.GetItemAmounts(req.ItemName);
            if (currentCount < req.RequireCount) return false;
        }
        return true;
    }

    private void ConsumeRequiredItems(CropDataSO crop)
    {
        foreach (var req in crop.RequireItems)
        {
            //TODO<김승태> : 아이템 소모하는 로직 추가
            // playerInven.아이템소모(req.ItemName, req.RequireCount);
        }
    }

    private bool AddInvetory()
    {
        if (selectedCrop == null || selectedCrop.harvestItemSo == ItemEnName.None)
        {
            Debug.LogWarning("수확할 작물이 없거나, 아이템정보가 설정되지 않았습니다.");
            return false;
        }
        // var item = selectedCrop.harvestItemSo;
        if (!GameManager.Instance.Item.ItemEnDataSO.TryGetValue(selectedCrop.harvestItemSo, out var item))
        {
            return false;
        }
        int remain = playerInven.AddItemSmart(item, selectedCrop.harvestItemAmounts);

        if (remain == 0)
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, item, 1);
            return true;
        }
        else
        {
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
            return false;
        }

    }

    public static void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
    public static void OnCloseKeyPressed() => m_isEscapeKeyPressed = true;
}