using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//# 비고 - 현재는 팝업창 하나로 아이템 획득/파괴/인벤토리가 꽉 찾을 시 동일하게 띄움
public class PopupInfoUI : MonoBehaviour
{
    private Image m_popupUIContainer;
    private TextMeshProUGUI m_popupText;

    private WaitForSeconds m_waitTime = new WaitForSeconds(1f);
    private Coroutine m_hideCoroutine;

    private void Awake()
    {
        var images = GetComponentsInChildren<Image>();
        m_popupUIContainer = images.Length > 1 ? images[1] : images[0];

        m_popupText = GetComponentInChildren<TextMeshProUGUI>();

        m_popupUIContainer.gameObject.SetActive(false);
    }

    private void Start() => GameManager.Instance.UI.SetPopupInfoUI(this);

    public void DisplayPopupView(PopupType popupType, ItemDataSO itemDataSO = null, int itemAmount = 0)
    {
        switch (popupType)
        {
            case PopupType.Acquired:
                m_popupText.text = $"{itemDataSO.ItemData.ItemName}({itemAmount})을 획득하였습니다.";
                break;
            case PopupType.Destroyed:
                m_popupText.text = $"{itemDataSO.ItemData.ItemName}({itemAmount})을 파괴하였습니다.";
                break;
            case PopupType.NotDestroyed:
                m_popupText.text = $"{itemDataSO.ItemData.ItemName}({itemAmount})을 파괴할 수 없습니다.";
                break;
            case PopupType.Full:
                m_popupText.text = "인벤토리가 가득 찼습니다.";
                break;
            case PopupType.NoneTile:
                m_popupText.text = "미개척지입니다.\n곡괭이가 필요합니다.";
                break;
            case PopupType.NeedTool:
                m_popupText.text = "개척지입니다.\n다른 도구가 필요합니다.";
                break;
            case PopupType.NeedFarmable:
                m_popupText.text = "경작지입니다.\n삽이 필요합니다.";
                break;
            case PopupType.NeedHammer:
                m_popupText.text = "방어시설입니다.\n망치가 필요합니다.";
                break;
            case PopupType.NeedWater:
                m_popupText.text = "급수시설입니다.\n양동이가 필요합니다.";
                break;
            case PopupType.Frontier:
                m_popupText.text = "타일이 개척지로 변경됐습니다.";
                break;
            case PopupType.Farmable:
                m_popupText.text = "타일이 경작지로 변경됐습니다.";
                break;
            case PopupType.DefenceArea:
                m_popupText.text = "타일이 방어시설로 변경됐습니다.";
                break;
            case PopupType.Water:
                m_popupText.text = "타일이 급수시설로 변경됐습니다.";
                break;
            case PopupType.ChangingState:
                m_popupText.text = "타일 상태를 변환 중입니다.";
                break;
            case PopupType.CantPlant:
                m_popupText.text = "현재는 재배가 불가능합니다.";
                break;
            case PopupType.CantExploit:
                m_popupText.text = "현재는 개척이 불가능합니다.";
                break;
            case PopupType.CantInteract:
                m_popupText.text = "현재는 상호작용이 불가능합니다.";
                break;
             case PopupType.CantBuildTurret:
                m_popupText.text = "현재는 터렛 설치가 불가능합니다.";
                break;
        }

        m_popupUIContainer.gameObject.SetActive(true);

        if (m_hideCoroutine != null) StopCoroutine(m_hideCoroutine);
        m_hideCoroutine = StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return m_waitTime;

        m_popupText.text = "";
        m_popupUIContainer.gameObject.SetActive(false);
    }
}