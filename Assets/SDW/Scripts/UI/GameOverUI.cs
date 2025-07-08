using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject m_panel;

    private void OnEnable()
    {
        m_panel.SetActive(false);
        GameManager.Instance.UI.SetCursorLock();
    }
}