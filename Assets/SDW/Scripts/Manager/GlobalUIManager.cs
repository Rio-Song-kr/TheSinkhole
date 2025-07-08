using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 전역 UI 요소들을 관리하는 매니저 클래스
/// </summary>
public class GlobalUIManager : MonoBehaviour
{
    [SerializeField] private GameObject m_crosshairUI;
    [SerializeField] private GameObject m_gameOverCanvas;
    [SerializeField] private Button m_retryButton;
    [SerializeField] private Button m_exitButton;
    private PopupInfoUI m_popupInfoUI;

    /// <summary>
    /// Popup 정보 UI 인스턴스를 반환
    /// </summary>
    public PopupInfoUI Popup => m_popupInfoUI;

    public void Awake()
    {
        GameManager.Instance.SetGlobalUIManager(this);
        m_retryButton.onClick.AddListener(SceneReload);
    }

    /// <summary>
    /// Popup 정보 UI를 설정
    /// </summary>
    /// <param name="popupInfoUI">설정할 팝업 UI 인스턴스</param>
    public void SetPopupInfoUI(PopupInfoUI popupInfoUI) => m_popupInfoUI = popupInfoUI;

    /// <summary>
    /// UI가 Close 되거나 게임 시작이 CursorLockMode = Locked;
    /// </summary>
    public void SetCursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_crosshairUI.SetActive(true);
    }

    /// <summary>
    /// UI가 Open될 때 CursorLockMode = Locked;
    /// </summary>
    public void SetCursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        m_crosshairUI.SetActive(false);
    }

    public void SceneReload()
    {
        EnableGameOverCanvas(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(0);
    }

    public void EnableGameOverCanvas(bool value) => m_gameOverCanvas.SetActive(value);
    public void Exit() => Application.Quit();
}