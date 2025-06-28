using UnityEngine;

/// <summary>
/// 게임 전반을 관리하는 Singleton 매니저 클래스
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    /// <summary>
    /// GameManager의 Singleton Instance를 반환
    /// </summary>
    public static GameManager Instance => m_instance;

    /// <summary>
    /// 전역 UI를 관리하는 매니저
    /// </summary>
    public GlobalUIManager UI { get; private set; }

    /// <summary>
    /// 아이템을 관리하는 매니저
    /// </summary>
    public ItemManager Item { get; private set; }

    public bool IsCursorLocked => Cursor.lockState == CursorLockMode.Locked;

    private bool m_isDay = true;
    public bool IsDay => m_isDay;

    [SerializeField] private GameObject m_crosshairUI;

    //todo 추후 통합시 이용
    // public static void CreateInstance()
    // {
    //     if (_instance == null)
    //     {
    //         var gameManagerPrefab = Resources.Load<GameManager>("GameManager");
    //         _instance = Instantiate(gameManagerPrefab);
    //         DontDestroyOnLoad(_instance);
    //     }
    // }

    /// <summary>
    /// Singleton Instance를 구현하고 필요한 컴포넌트들을 초기화
    /// </summary>
    private void Awake()
    {
        if (m_instance != null && !m_instance.Equals(this))
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        DontDestroyOnLoad(gameObject);

        UI = GetComponent<GlobalUIManager>();
        Item = GetComponent<ItemManager>();
    }

    private void Start()
    {
        int targetWidth = 1920;
        int targetHeight = 1080;

        Screen.SetResolution(targetWidth, targetHeight, true);
    }

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
}