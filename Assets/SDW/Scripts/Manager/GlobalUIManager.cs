using UnityEngine;

/// <summary>
/// 전역 UI 요소들을 관리하는 매니저 클래스
/// </summary>
public class GlobalUIManager : MonoBehaviour
{
    private PopupInfoUI m_popupInfoUI;

    /// <summary>
    /// Popup 정보 UI 인스턴스를 반환
    /// </summary>
    public PopupInfoUI Popup => m_popupInfoUI;

    /// <summary>
    /// Popup 정보 UI를 설정
    /// </summary>
    /// <param name="popupInfoUI">설정할 팝업 UI 인스턴스</param>
    public void SetPopupInfoUI(PopupInfoUI popupInfoUI) => m_popupInfoUI = popupInfoUI;
}