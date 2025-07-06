using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class TurretUI : Singleton<TurretUI>
{
    [Header("Turret")]
    [SerializeField] private TurretSo selectedTurret;
    public TurretSo[] TurretList;

    [Header("UI")]
    public GameObject TurretUIGO;
    public bool GetActiveself() => TurretUIGO.activeSelf;
    private bool isBuiltOnce = false;

    //터렛 Detail
    public GameObject DetailGO;
    [SerializeField] private Image turretImg;
    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text turretDesc;
    [SerializeField] private TMP_Text turretRequiedTime;


    [SerializeField] private TMP_Text m_statusText; //상태 메세지
    public Image ProgressBarImg;
    public Button[] turretButtons;

    private float pressTimer = 0f;
    private float pressDuration = 5f;
    private bool isPressingE = false;
    //스크롤뷰
    [SerializeField] private GameObject turretBtnPrefab;
    [SerializeField] private Transform scrollViewContentPos; //스크롤뷰 컨텐츠 위치

    //오픈 관련 이벤트 처리
    public event Action<bool> OnIsUIOpen;


    [Header("Tile")]
    [SerializeField] private TurretTile currentTile;
    [SerializeField] private float builtTimer = 0f;

    //필요 아이템 목록
    [SerializeField] private GameObject requireBtnPrefab;
    [SerializeField] private Transform contentTransfrom; //필요아이템 목록 컨텐츠 위치

    //인벤토리
    [SerializeField] private Inventory playerInven;

    private static bool m_isIneractionKeyPressed;
    private static bool m_isEscapeKeyPressed;


    void Start()
    {
        ScrollViewSetting();
        m_statusText.text = "";
        ProgressBarImg.fillAmount = 0f;
        TurretUIGO.SetActive(false);
    }
    void Update()
    {
        if (currentTile == null) return;

        if (m_isEscapeKeyPressed && !m_isIneractionKeyPressed)
        {
            CloseUI();
            return;
        }
        if (!GameTimer.IsDay)
        {
            if (currentTile.IsBuild())
            {
                // DevelopingProgress();
                BuildingProgress();
            }
            else if (pressTimer > 0f)
            {
                CancelBuilding();
            }
            if (m_isIneractionKeyPressed)
            {
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.CantBuildTurret);
            }
            return;
        }
        if (currentTile.IsBuild())
        {
            BuildingProgress();
        }
        else if (selectedTurret != null)
        {
            if (m_isIneractionKeyPressed)
            {
                if (!HasRequiredItems(selectedTurret))
                {
                    m_statusText.text = "재료 아이템이 부족합니다.";
                    return;
                }

                isPressingE = true;
                Debug.Log($"isPressingE : {isPressingE}");
                pressTimer += Time.deltaTime;
                ProgressBarImg.fillAmount = pressTimer / pressDuration;
                m_statusText.text = $"개척 준비 중... {FormatingTime.FormatSecTime(pressDuration - pressTimer)}초";

                if (pressTimer >= pressDuration)
                {
                    pressTimer = 0f;
                    isPressingE = false;

                    if (!GameTimer.IsDay) return; // 낮이 아닐 경우

                    foreach (var req in selectedTurret.RequireItems)
                    {
                        if (req.ItemName == ItemEnName.None) continue;

                        bool result = playerInven.RemoveItemAmounts(req.ItemName, req.RequireCount);
                        if (!result)
                        {
                            m_statusText.text = "아이템 소모 실패";
                            return;
                        }
                    }
                    StartBuilding(selectedTurret);

                }
            }
            else if (pressTimer > 0f)
            {
                CancelBuilding();
            }
        }
    }

     private void BuildingProgress()
    {
        var turret = currentTile.GetBuiltTurret();
        DisplayTurretDetail(turret);

        if (isBuiltOnce)
        {
            m_statusText.text = "설치 완료된 타일입니다.";
            ProgressBarImg.fillAmount = 0f;
            return;
        }

        float remain = currentTile.GetRemainingInstallTime();

        if (currentTile.IsInstalling())
        {
            m_statusText.text = $"설치중 {FormatingTime.FormatMinTime(remain)}";
            ProgressBarImg.fillAmount = 1f;
        }
        else
        {
            m_statusText.text = "설치 완료! 배치하려면 [E]키를 누르세요";
            ProgressBarImg.fillAmount = 1f;

            if (m_isIneractionKeyPressed)
            {
                Build();
            }
        }
    }

    void CancelBuilding()
    {
        //상태 전부 초기화

        pressTimer = 0f;
        ProgressBarImg.fillAmount = 0f;
        isPressingE = false;
        m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";
    }
    public void SetTile(TurretTile tile)
    {
        currentTile = tile;
        isBuiltOnce = tile.IsBuild();
        //해당 타일이 이미 설치중이라면 해당 터렛 표시
        if (tile.IsBuild())
        {
            DisplayTurretDetail(tile.GetBuiltTurret());
        }
        else
        {
            if (selectedTurret != null)
            {
                DisplayTurretDetail(selectedTurret);
                m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

                ProgressBarImg.fillAmount = 0f;
            }
        }
    }

    public void OpenUI(TurretTile tile)
    {
        if (!GameTimer.IsDay) return;

        currentTile = tile;
        selectedTurret = null;
        pressTimer = 0f;
        isPressingE = false;
        isBuiltOnce = tile.IsBuild();

        TurretUIGO.SetActive(true);
        GameManager.Instance.SetCursorUnlock();
        OnIsUIOpen?.Invoke(true);

        m_isIneractionKeyPressed = false;

        if (selectedTurret == null)
        {
            m_statusText.text = " 터렛을 선택해주세요";
            ProgressBarImg.fillAmount = 0f;
            // DetailGO.SetActive(false);
            return;
        }

        DisplayTurretDetail(selectedTurret);


        if (currentTile != null && !currentTile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

            ProgressBarImg.fillAmount = 0f;
        }
    }
    public void CloseUI()
    {
        currentTile = null;

        TurretUIGO.SetActive(false);
        OnIsUIOpen?.Invoke(false);
        GameManager.Instance.SetCursorLock();
        m_isEscapeKeyPressed = false;
    }
    public void SelectTurret(TurretSo turret)
    {
        //설치 돼있다면 다른 터렛은 설치하지 못하도록
        if (currentTile != null && currentTile.IsBuild())
        {
            if (currentTile.GetBuiltTurret() != turret) return;
        }
        selectedTurret = turret;
        DisplayTurretDetail(turret);

        if (currentTile != null && !currentTile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요. ";

            ProgressBarImg.fillAmount = 0f;
        }
    }
    private void StartBuilding(TurretSo so)
    {
        currentTile.StartBuiltTurret(so);
        builtTimer = so.buildingTime;

        m_statusText.text = $"제작중 {builtTimer}";
        ProgressBarImg.fillAmount = 1f;
        foreach (var btn in turretButtons)
        {
            btn.interactable = false
            ;
        }
    }
    public void DisplayTurretDetail(TurretSo data)
    {
        DetailGO.SetActive(true);

        turretImg.sprite = data.TurretImg;
        turretName.text = data.TurretName;
        turretRequiedTime.text = $"{data.buildingTime} Seconds";
        turretDesc.text = $"타워 상세정보 : {data.TurretDesc}초 \n 공격력: {data.Atk} \n 사거리 : {data.distance} %";

        // 기존에 있던 RequireItem 오브젝트 제거
        foreach (Transform child in contentTransfrom)
        {
            Destroy(child.gameObject);
        }
        foreach (var req in data.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            var go = Instantiate(requireBtnPrefab, contentTransfrom);
            var ItemUI = go.GetComponent<RequireItem>();

            int currentAmount = playerInven.GetItemAmounts(req.ItemName);

            if (GameManager.Instance.Item.ItemEnDataSO.TryGetValue(req.ItemName, out var item))
            {
                ItemUI.Set(item.Icon, item.ItemData.ItemName, req.RequireCount, currentAmount);
            }

        }

    }
    public void ScrollViewSetting()
    {
        List<Button> btnList = new();
        foreach (Transform child in scrollViewContentPos)
        {
            Destroy(child.gameObject);
        }
        foreach (var turret in TurretList)
        {
            GameObject go = Instantiate(turretBtnPrefab, scrollViewContentPos);
            TurretBtn btn = go.GetComponent<TurretBtn>();
            btn.Init(turret);
            btnList.Add(btn.Btn);
        }
        turretButtons = btnList.ToArray();
    }

    //수확
    private void Build()
    {
        if (!GameTimer.IsDay) return; //낮이 아니면

        if (isBuiltOnce) return;
        isBuiltOnce = true;

        var turretSo = currentTile.GetBuiltTurret();
        if (turretSo == null) return;

        var go = Instantiate(turretSo.turretPrefab, currentTile.transform.position + Vector3.up * 0.1f, Quaternion.identity);

        var turret = go.GetComponent<Turret>();
        if (turret != null)
        {
            turret.Init(turretSo);
        }

        currentTile.EndBuilding();

        //초기화
        builtTimer = 0f;
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = $"설치 완료된 타일입니다.";

        // foreach (var btn in turretButtons)
        // {
        //     btn.interactable = true;
        // }
    }

    //재료 아이템 체크
    private bool HasRequiredItems(TurretSo so)
    {
        foreach (var req in so.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            int currentCount = playerInven.GetItemAmounts(req.ItemName);
            if (currentCount < req.RequireCount) return false;
        }
        return true;
    }


    public static void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
    public static void OnCloseKeyPressed() => m_isEscapeKeyPressed = true;


}