using System.Collections;
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

    public AudioManager Audio { get; private set; }

    public bool IsCursorLocked => Cursor.lockState == CursorLockMode.Locked;

    /// <summary>
    /// 낮 또는 방의 상태를 나타내기 위한 프로퍼티
    /// </summary>
    private bool m_isGameOver;

    /// <summary>
    /// GameOver 여부 확인
    /// </summary>
    public bool IsGameOver => m_isGameOver;

    private bool m_isRun = false;

    //todo 추후 통합시 이용
    public static void CreateInstance()
    {
        if (m_instance == null)
        {
            var gameManagerPrefab = Resources.Load<GameManager>("GameManager");
            m_instance = Instantiate(gameManagerPrefab);
            DontDestroyOnLoad(m_instance);
        }
    }

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
    }

    private void Start()
    {
        int targetWidth = 1920;
        int targetHeight = 1080;

        Screen.SetResolution(targetWidth, targetHeight, true);
        Audio = GetComponent<AudioManager>();
        Audio.PlayBGM(AudioClipName.Intro_Rain);

        StartCoroutine(PlaySFX());
    }

    private IEnumerator PlaySFX()
    {
        yield return new WaitForSeconds(5f);

        Audio.PlaySFX(AudioClipName.M_Hole, Vector3.zero);
    }

    private void Update()
    {
        if (!m_isGameOver) return;

        if (m_isRun) return;
        m_isRun = true;
        UI.SetCursorUnlock();
        UI.EnableGameOverCanvas(true);
    }

    public void SetGameOver() => m_isGameOver = true;

    public void SetGlobalUIManager(GlobalUIManager globalUIManager) => UI = globalUIManager;
    public void SetItemManager(ItemManager itemManager) => Item = itemManager;
    public void SetMonsterManager(MonsterManager monsterManager) => Monster = monsterManager;
    public void SetTileManager(TileManager tileManager) => Tile = tileManager;
    public void SetShelterManager(ShelterManager shelterManager) => Shelter = shelterManager;
    public void SetActionManager(ActionManager actionManager) => Action = actionManager;
    public void SetEffectManager(EffectManager effectManager) => Effect = effectManager;
    public void SetItemRecipeManager(ItemRecipeManager itemRecipeManager) => Recipe = itemRecipeManager;
}