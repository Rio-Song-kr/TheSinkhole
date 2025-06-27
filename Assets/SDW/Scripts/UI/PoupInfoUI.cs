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
            case PopupType.Full:
                m_popupText.text = "인벤토리가 가득 찼습니다.";
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