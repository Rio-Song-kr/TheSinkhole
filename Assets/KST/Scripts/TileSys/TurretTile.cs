using UnityEngine;

public class TurretTile : MonoBehaviour, IToolInteractable
{
    [Header("Status")]
    [SerializeField]private bool m_isBuild; //터렛 설치되어 있으면 true, 아니면 false
    [SerializeField]private bool m_isPlayerOnTurretTile; //터렛타일에 플레이어가 있으면 true, 없으면 false
    public bool IsBuild() => m_isBuild;
    public bool IsPlayerOnTile() => m_isPlayerOnTurretTile; 

    [Header("UI")]
    //상호작용 UI
    public GameObject InteractUiText;
    private bool m_isInteract;

    [SerializeField] private TurretSo builtTurret;
    public TurretSo GetBuiltTurret() => builtTurret;
    //TODO: 터렛UI 이벤트 구독 처리
    
    void OnEnable()
    {
        TurretUI.Instance.OnIsUIOpen +=SetInteraction;
    }
    void OnDisable()
    {
        TurretUI.Instance.OnIsUIOpen -=SetInteraction;
    }

    #region 상호작용 인터페이스 구현
    public interactType GetInteractType()
    {
        return interactType.PressE;
    }

    public bool CanInteract(ToolType toolType)
    {
        // return m_isPlayerOnFarmTile && !m_isPlanted && toolType == ToolType.Shovel;

        //TODO: 만약 플레이어가 해머 들 필요 없으면 해당 조건 수정 필요
        return m_isPlayerOnTurretTile && toolType == ToolType.Hammer;
    }
    public void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None) return;

        TurretUI.Instance.OpenUI();
        TurretUI.Instance.SetTile(this);
        InteractUiText.SetActive(false);

    }
    #endregion

    //터렛 설치 메서드
    public void StartBuiltTurret(TurretSo so)
    {
        m_isBuild = true;
        builtTurret = so;
    }

    void SetInteraction(bool _status)
    {
        InteractUiText.SetActive(_status);
    }

    #region 충돌처리

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnTurretTile = true;
            if (!TurretUI.Instance.GetActiveself())
            {
                InteractUiText.SetActive(true);
            }
            Interaction player = other.GetComponent<Interaction>();
            player?.RegisterTrigger(this);

        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnTurretTile = false;
            InteractUiText.SetActive(false);
            Interaction player = other.GetComponent<Interaction>();
            player?.ClearTrigger(this);
        }

    }
    #endregion

}
