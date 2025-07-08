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
    private bool isComplete = false;

    [Header("Detail")]
    public GameObject DetailGO;
    [SerializeField] private Image turretImg;
    [SerializeField] private TMP_Text turretName;
    [SerializeField] private TMP_Text turretDesc;
    [SerializeField] private TMP_Text turretRequiedTime;
    [SerializeField] private TMP_Text m_statusText;
    public Image ProgressBarImg;

    [Header("ScrollView")]
    [SerializeField] private GameObject turretBtnPrefab;
    [SerializeField] private Transform scrollViewContentPos;
    public Button[] turretButtons;

    [Header("Tile")]
    [SerializeField] private TurretTile currentTile;
    [SerializeField] private float builtTimer = 0f;

    [Header("Require Items")]
    [SerializeField] private GameObject requireBtnPrefab;
    [SerializeField] private Transform contentTransfrom;

    [Header("Inventory")]
    [SerializeField] private Inventory playerInven;

    [SerializeField]private float pressTimer = 0f;
    private float pressDuration = 5f;

    private static bool m_isIneractionKeyPressed;
    private static bool m_isEscapeKeyPressed;

    public event Action<bool> OnIsUIOpen;

    private void Start()
    {
        ScrollViewSetting();
        m_statusText.text = "";
        ProgressBarImg.fillAmount = 0f;
        TurretUIGO.SetActive(false);
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
            if (currentTile.IsBuild()) BuildingProgress();
            else if (pressTimer > 0f) CancelBuilding();

            if (m_isIneractionKeyPressed)
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.CantBuildTurret);
            return;
        }

        if (currentTile.IsBuild()) BuildingProgress();
        else if (selectedTurret != null && m_isIneractionKeyPressed)
        {
            if (!HasRequiredItems(selectedTurret))
            {
                m_statusText.text = "재료 아이템이 부족합니다.";
                return;
            }

            pressTimer += Time.deltaTime;
            ProgressBarImg.fillAmount = pressTimer / pressDuration;
            m_statusText.text = $"설치 준비 중... {FormatingTime.FormatSecTime(pressDuration - pressTimer)}초";
            ProgressBarImg.color = Color.white;

            if (pressTimer >= pressDuration)
            {
                pressTimer = 0f;

                foreach (var req in selectedTurret.RequireItems)
                {
                    if (req.ItemName == ItemEnName.None) continue;
                    if (!playerInven.RemoveItemAmounts(req.ItemName, req.RequireCount))
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

    private void BuildingProgress()
    {
        var turret = currentTile.GetBuiltTurret();
        DisplayTurretDetail(turret);

        float remain = currentTile.GetRemainingInstallTime();

        if (currentTile.IsInstalling())
        {
            m_statusText.text = $"설치중 {FormatingTime.FormatMinTime(remain)}";
            ProgressBarImg.fillAmount = 1f;
        }
        else if (!currentTile.IsDeployed() && currentTile.IsBuild()) // 배치 전 상태
        {
            m_statusText.text = "설치 완료! 배치하려면 [E]키를 누르세요";
            ProgressBarImg.fillAmount = 1f;
            ProgressBarImg.color = ColorUtil.Hexcode("#8CEF8F", Color.green);

            isComplete = true;

            if (m_isIneractionKeyPressed)
                Build();
        }
        else if (currentTile.IsDeployed()) // 배치 완료 상태
        {
            m_statusText.text = "설치 완료된 타일입니다.";
            ProgressBarImg.fillAmount = 0f;
        }
    }

    private void StartBuilding(TurretSo so)
    {
        currentTile.StartBuiltTurret(so);
        builtTimer = so.buildingTime;

        m_statusText.text = $"제작중 {builtTimer}";
        ProgressBarImg.fillAmount = 1f;

        if (GameManager.Instance.Action.ActionIdEffect.TryGetValue(50502, out var effect))
        {
            GameManager.Instance.Action.OnActionEffect?.Invoke(effect);
        }
    }

    private void Build()
    {
        if (!GameTimer.IsDay) return;
        if (isBuiltOnce) return;

        isBuiltOnce = true;

        var turretSo = currentTile.GetBuiltTurret();
        if (turretSo == null) return;

        // var go = Instantiate(turretSo.turretPrefab, currentTile.transform.position + Vector3.up * 0.1f, Quaternion.identity);
        // var turret = go.GetComponent<Turret>();
        var turret = currentTile.GetTurretObject();
        if (turret == null)
        {
            Debug.Log("터렛이 없음");
            return;
        }
        turret.gameObject.SetActive(true);
        turret.Init(turretSo);

        currentTile.EndBuilding();
        currentTile.SetDeployed(true);

        builtTimer = 0f;
        ProgressBarImg.fillAmount = 0f;
        ProgressBarImg.color = ColorUtil.Hexcode("#865A5A", Color.red);
        m_statusText.text = "설치 완료된 타일입니다.";
    }

    private void CancelBuilding()
    {
        pressTimer = 0f;
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요.";
    }

    public void OpenUI(TurretTile tile)
    {
        if (!GameTimer.IsDay) return;

        currentTile = tile;
        pressTimer = 0f;
        isBuiltOnce = false; // 중요: Open 시점에는 false로 초기화

        TurretUIGO.SetActive(true);
        GameManager.Instance.SetCursorUnlock();
        OnIsUIOpen?.Invoke(true);
        m_isIneractionKeyPressed = false;

        if (selectedTurret == null && TurretList != null && TurretList.Length > 0)
        {
            SelectTurret(TurretList[0]);
        }

        if (selectedTurret == null)
        {
            m_statusText.text = "터렛을 선택해주세요.";
            ProgressBarImg.fillAmount = 0f;
            return;
        }

        DisplayTurretDetail(selectedTurret);
        if (!tile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요.";
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

    public void SetTile(TurretTile tile)
    {
        currentTile = tile;

        if (tile.IsBuild())
        {
            DisplayTurretDetail(tile.GetBuiltTurret());
        }
        else if (selectedTurret != null)
        {
            DisplayTurretDetail(selectedTurret);
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요.";
            ProgressBarImg.fillAmount = 0f;
        }
    }

    public void SelectTurret(TurretSo turret)
    {
        if (currentTile != null && currentTile.IsBuild())
        {
            if (currentTile.GetBuiltTurret() != turret) return;
        }

        selectedTurret = turret;
        DisplayTurretDetail(turret);

        if (currentTile != null && !currentTile.IsBuild())
        {
            m_statusText.text = $"제작하려면 [E]키를 {pressDuration}초 동안 눌러주세요.";
            ProgressBarImg.fillAmount = 0f;
        }
    }

    public void DisplayTurretDetail(TurretSo data)
    {
        DetailGO.SetActive(true);

        turretImg.sprite = data.TurretImg;
        turretName.text = data.TurretName;
        turretRequiedTime.text = $"{data.buildingTime} Seconds";
        turretDesc.text = $"타워 상세정보 : {data.TurretDesc}초 \n 공격력: {data.Atk} \n 사거리 : {data.distance} %";

        foreach (Transform child in contentTransfrom)
            Destroy(child.gameObject);

        foreach (var req in data.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            var go = Instantiate(requireBtnPrefab, contentTransfrom);
            var itemUI = go.GetComponent<RequireItem>();

            if (GameManager.Instance.Item.ItemEnDataSO.TryGetValue(req.ItemName, out var item))
            {
                int currentAmount = playerInven.GetItemAmounts(req.ItemName);
                itemUI.Set(item.Icon, item.ItemData.ItemName, req.RequireCount, currentAmount);
            }
        }
    }

    public void ScrollViewSetting()
    {
        var btnList = new List<Button>();
        foreach (Transform child in scrollViewContentPos)
            Destroy(child.gameObject);

        foreach (var turret in TurretList)
        {
            var go = Instantiate(turretBtnPrefab, scrollViewContentPos);
            var btn = go.GetComponent<TurretBtn>();
            btn.Init(turret);
            btnList.Add(btn.Btn);
        }

        turretButtons = btnList.ToArray();
    }

    private bool HasRequiredItems(TurretSo so)
    {
        foreach (var req in so.RequireItems)
        {
            if (req.ItemName == ItemEnName.None) continue;

            if (playerInven.GetItemAmounts(req.ItemName) < req.RequireCount)
                return false;
        }
        return true;
    }

    public static void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
    public static void OnCloseKeyPressed() => m_isEscapeKeyPressed = true;
}