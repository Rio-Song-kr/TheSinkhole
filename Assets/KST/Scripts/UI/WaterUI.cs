using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class WaterUI : Singleton<WaterUI>
{
    [Header("UI")]
    public GameObject waterUIGO;
    public bool GetActiveself() => waterUIGO.activeSelf;

    [SerializeField] private TMP_Text m_statusText;
    public Image ProgressBarImg;

    //Tile
    private WaterTile currentTile;

    private float pressTimer = 0f;
    private float pressDuration = 5f;
    // private bool isPressingE = false;

    private static bool m_isIneractionKeyPressed;
    private static bool m_isEscapeKeyPressed;
    public event Action<bool> OnIsUIOpen;

    //인벤토리
    [SerializeField] private Inventory playerInven;

    private void Start()
    {
        m_statusText.text = "";
        ProgressBarImg.fillAmount = 0f;
        waterUIGO.SetActive(false);
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
            if (currentTile.IsWatering())
            {
                float remain = currentTile.GetRemainingWaterTime();

                m_statusText.text = $"급수중 {FormatingTime.FormatMinTime(remain)}";
                ProgressBarImg.fillAmount = 1f;
            }
            else if (currentTile.IsWatered())
            {
                m_statusText.text = "급수 완료! [E]키를 누르세요";
                ProgressBarImg.fillAmount = 1f;

                if (m_isIneractionKeyPressed)
                {
                    Harvest();
                    Debug.Log("급수");
                }
            }
            else if (pressTimer > 0f)
            {
                CancelWatering();
            }
            if (m_isIneractionKeyPressed)
            {
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.CantInteract);
            }
            return;
        }
        if (currentTile.IsWatering())
        {
            float remain = currentTile.GetRemainingWaterTime();

            m_statusText.text = $"급수중 {FormatingTime.FormatMinTime(remain)}";
            // ProgressBarImg.color = ColorUtil.Hexcode("#8CB4EF", Color.blue);
            ProgressBarImg.color = Color.white;
            ProgressBarImg.fillAmount = 1f;
            return;
        }
        if (currentTile.IsWatered())
        {
            m_statusText.text = "급수 완료! [E]키를 누르세요";
            ProgressBarImg.color = ColorUtil.Hexcode("#8CEF8F", Color.green);
            ProgressBarImg.fillAmount = 1f;

            if (m_isIneractionKeyPressed)
            {
                Harvest();
                Debug.Log("급수");
            }
            return;
        }

        if (m_isIneractionKeyPressed)
        {
            // isPressingE = true;
            pressTimer += Time.deltaTime;
            ProgressBarImg.fillAmount = pressTimer / pressDuration;
            m_statusText.text = $"급수 준비 중. . . {FormatingTime.FormatSecTime(pressDuration - pressTimer)}초";

            if (pressTimer >= pressDuration)
            {
                StartWatering();
            }
        }
        else if (pressTimer > 0f)
        {
            CancelWatering();
        }
    }

    public void OpenUI(WaterTile tile)
    {
        if (!GameTimer.IsDay) return;

        currentTile = tile;
        pressTimer = 0f;
        // isPressingE = false;

        waterUIGO.SetActive(true);
        GameManager.Instance.SetCursorUnlock();
        OnIsUIOpen?.Invoke(true);

        m_isIneractionKeyPressed = false;

        m_statusText.text = "[E]키를 눌러서 급수하세요.";
        ProgressBarImg.fillAmount = 0f;
    }

    public void CloseUI()
    {
        currentTile = null;
        waterUIGO.SetActive(false);
        OnIsUIOpen?.Invoke(false);
        GameManager.Instance.SetCursorLock();
        m_isEscapeKeyPressed = false;
    }

    //급수 시작
    private void StartWatering()
    {
        currentTile.StartWatering();
        pressTimer = 0f;
        // isPressingE = false;

        m_statusText.text = "급수 시작됨";
        ProgressBarImg.fillAmount = 1f;
    }

    private void CancelWatering()
    {
        //상태 전부 초기화
        pressTimer = 0f;
        // isPressingE = false;
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = "[E]키를 눌러서 급수하세요.";
    }

    private void Harvest()
    {
        if (!GameTimer.IsDay) return; //낮이 아니면

        if (!AddInventory())
        {
            ProgressBarImg.fillAmount = 1f;
            return; // 인벤에 예외 발생시.
        }

        ProgressBarImg.color = ColorUtil.Hexcode("#865A5A", Color.red);
        currentTile.EndWatering();
        ProgressBarImg.fillAmount = 0f;
        m_statusText.text = "[E]키를 눌러서 급수하세요.";
        pressTimer = 0f;
        // isPressingE = false;
    }

    private bool AddInventory()
    {
        if (!GameManager.Instance.Item.ItemEnDataSO.TryGetValue(ItemEnName.Water, out var item))
        {
            return false;
        }

        //1. 슬롯 검사
        int requireSlots = 1;

        if (playerInven.GetRemainingSlots() < requireSlots)
        {
            m_statusText.text = "인벤토리에 슬롯이 부족합니다.";
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
            return false;
        }

        //2. 아이템 추가
        int remain = playerInven.AddItemSmart(item, 1);
        if (remain > 0)
        {
            m_statusText.text = "인벤토리에 공간이 부족합니다.";
            GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Full);
            return false;
        }

        //3. 인벤토리에 추가
        GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Acquired, item, 1);
        return true;
    }

    public static void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
    public static void OnCloseKeyPressed() => m_isEscapeKeyPressed = true;
}