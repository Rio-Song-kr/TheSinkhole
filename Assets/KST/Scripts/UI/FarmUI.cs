using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmUI : Singleton<FarmUI>
{
    [Header("Crop")]
    [SerializeField] private CropDataSO selectedCrop;
    public CropDataSO[] CropList;

    [Header("UI")]
    public GameObject FarmUIGO;

    //작물 Detail
    [SerializeField] private Image cropImg;
    [SerializeField] private TMP_Text cropName;
    [SerializeField] private TMP_Text cropDesc;
    // [SerializeField] private TMP_Text cropTime;
    [SerializeField] private TMP_Text m_statusText;
    public Image PrograssBarImg;
    public Button[] cropButtons;

    private float pressTimer = 0f;
    private float pressDuration = 5f;
    private bool isPressingE = false;

    [Header("Tile")]
    [SerializeField] private FarmTile currentTile;
    private float growTimer = 0f;

    // void Start()
    // {
    //     m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요. ";
    // }

    void Update()
    {
        if (!FarmUIGO.activeSelf || currentTile == null) return;//FarmUI가 비활성화 돼 있다면 무시.

        if (currentTile.IsPlanted()) // 해당 타일에 작물이 심어져있는 상태가 아니라면,
        {
            var growingCrop = currentTile.GetGrownCrop();

            if (growingCrop == selectedCrop)
            {
                growTimer -= Time.deltaTime;
                if (growTimer < 0f) growTimer = 0f;
                m_statusText.text = $"재배중 {growTimer}";
            }
            else
            {
                m_statusText.text = "해당 작물은 현재 재배할 수 없습니다.";
            }
            return;
        }

        //재배 가능한 상태
        if (Input.GetKey(KeyCode.E))
        {
            isPressingE = true;
            pressTimer += Time.deltaTime;
            PrograssBarImg.fillAmount = pressTimer / pressDuration;
            m_statusText.text = $"작물 재배 중. . .";


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
    void CancelPlanting()
    {
        //상태 전부 초기화

        pressTimer = 0f;
        PrograssBarImg.fillAmount = 0f;
        m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요. ";
        isPressingE = false;
    }
    public void SetTile(FarmTile tile)
    {
        currentTile = tile;
        //해당 타일이 이미 재배중이라면 해당 작물 표시
        if (tile.IsPlanted())
        {
            DisplayCropDetail(tile.GetGrownCrop());
            growTimer = tile.GetGrownCrop().growTime;
        }
        else
        {
            if (selectedCrop != null)
            {
                DisplayCropDetail(selectedCrop);
                m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요. ";
                PrograssBarImg.fillAmount = 0f;
            }
        }
    }

    public void OpenUI()
    {
        FarmUIGO.SetActive(true);
        if (selectedCrop != null)
        {
            DisplayCropDetail(selectedCrop);
        }
        if (currentTile != null && !currentTile.IsPlanted())
        {
            m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요. ";
            PrograssBarImg.fillAmount=0f;
        }
    }
    public void CloseUI()
    {
        FarmUIGO.SetActive(false);
    }
    public void SelectCrop(CropDataSO crop)
    {
        //재배 중일 경우 다른 작물 선택 불가하도록
        if (currentTile != null && currentTile.IsPlanted())
        {
            if (currentTile.GetGrownCrop() != crop) return;
        }
        selectedCrop = crop;
        DisplayCropDetail(crop);

        if (currentTile != null && !currentTile.IsPlanted())
        {
            m_statusText.text = $"재배하기 [E]키를 {pressDuration}초 동안 눌러주세요. ";
            PrograssBarImg.fillAmount=0f;
        }
    }
    private void StartGrowing(CropDataSO selectedCrop)
    {
        currentTile.StartPlanting(selectedCrop);
        growTimer = selectedCrop.growTime;

        m_statusText.text = $"재배중 {growTimer}";
        PrograssBarImg.fillAmount = 1f;
        foreach (var btn in cropButtons)
        {
            btn.interactable = false;
        }
    }
    public void DisplayCropDetail(CropDataSO data)
    {
        cropImg.sprite = data.cropImg;
        cropName.text = data.cropName;
        // cropDesc.text = $"Time Required : {data.growTime} \n {data.cropDesc} \n Hungry : {data.cropEffect} %";
        cropDesc.text = $"소요 시간 : {data.growTime}초 \n {data.cropDesc} \n 배고픔 : {data.cropEffect} %";
    }
}