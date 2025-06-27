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

    //todo 추후 통합시 아용
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
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        DontDestroyOnLoad(gameObject);

        UI = GetComponent<GlobalUIManager>();
        Item = GetComponent<ItemManager>();
    }
}