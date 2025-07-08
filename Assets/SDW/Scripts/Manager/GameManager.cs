using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// 몬스터를 관리하는 매니저
    /// </summary>
    public MonsterManager Monster { get; private set; }

    /// <summary>
    /// 타일을 관리하는 매니저
    /// </summary>
    public TileManager Tile { get; private set; }

    /// <summary>
    /// 쉘터 데이터를 관리하는 매니저
    /// </summary>
    public ShelterManager Shelter { get; private set; }

    /// <summary>
    /// Action 데이터를 관리하는 매니저
    /// </summary>
    public ActionManager Action { get; private set; }

    /// <summary>
    /// Effect 데이터를 관리하는 매니저
    /// </summary>
    public EffectManager Effect { get; private set; }

    /// <summary>
    /// Recipe 데이터를 관리하는 매니저
    /// </summary>
    public ItemRecipeManager Recipe { get; private set; }

    public bool IsCursorLocked => Cursor.lockState == CursorLockMode.Locked;

    /// <summary>
    /// 낮 또는 방의 상태를 나타내기 위한 프로퍼티
    /// </summary>
    private bool m_isGameOver;

    /// <summary>
    /// GameOver 여부 확인
    /// </summary>
    public bool IsGameOver => m_isGameOver;

    [SerializeField] private GameObject m_gameOverCanvas;

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
        Monster = GetComponent<MonsterManager>();
        Tile = GetComponent<TileManager>();
        Shelter = GetComponent<ShelterManager>();
        Action = GetComponent<ActionManager>();
        Effect = GetComponent<EffectManager>();
        Recipe = GetComponent<ItemRecipeManager>();
    }

    private void Start()
    {
        int targetWidth = 1920;
        int targetHeight = 1080;

        Screen.SetResolution(targetWidth, targetHeight, true);
    }

    private void Update()
    {
        if (!m_isGameOver) return;

        SetCursorUnlock();
        m_gameOverCanvas.SetActive(true);
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

    public void SetGameOver() => m_isGameOver = true;

    public void SceneReload() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    public void Exit() => Application.Quit();
}