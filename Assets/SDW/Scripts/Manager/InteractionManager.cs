using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_centerTextObject;
    [SerializeField] private TextMeshProUGUI m_centerText;
    [SerializeField] private Image m_crosshairObject;

    private InteractionType m_currentInteractionType = InteractionType.None;
    private string m_currentText = "";
    private bool m_isTextActive = false;
    private bool m_isCrosshairActive = true;

    public void SetInteractionUI(InteractionType interactionType, bool showText, string text = "", bool showCrosshair = true)
    {
        //# 현재 상호작용보다 우선순위가 높거나 같은 경우에만 UI 업데이트
        if (GetPriority(interactionType) >= GetPriority(m_currentInteractionType))
        {
            m_currentInteractionType = interactionType;
            m_currentText = text;
            m_isTextActive = showText;
            m_isCrosshairActive = showCrosshair;

            UpdateUI();
        }
    }

    public void ClearInteractionUI(InteractionType interactionType)
    {
        //# 현재 상호작용 타입과 일치하는 경우에만 클리어
        if (m_currentInteractionType == interactionType)
        {
            m_currentInteractionType = InteractionType.None;
            m_currentText = "";
            m_isTextActive = false;
            m_isCrosshairActive = GameManager.Instance.IsCursorLocked;

            UpdateUI();
        }
    }

    private int GetPriority(InteractionType interactionType)
    {
        return interactionType switch
        {
            InteractionType.Item => 3,
            InteractionType.ResourceItem => 3,
            InteractionType.Shelter => 2,
            InteractionType.Tile => 1,
            InteractionType.None => 0,
            _ => 0
        };
    }

    private void UpdateUI()
    {
        m_centerTextObject.SetActive(m_isTextActive);
        m_centerText.text = m_currentText;
        m_crosshairObject.enabled = m_isCrosshairActive;
    }

    public bool IsCurrentInteraction(InteractionType interactionType) => m_currentInteractionType == interactionType;
}