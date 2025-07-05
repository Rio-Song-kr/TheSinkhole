using UnityEngine;

public class TurretTile : Tile
{
    [Header("Status")]
    [SerializeField] private bool m_isTurretBuilt; //터렛 설치되어 있으면 true, 아니면 false
    [SerializeField] private bool m_isPlayerOnTurretTile; //터렛타일에 플레이어가 있으면 true, 없으면 false
    public bool IsBuild() => m_isTurretBuilt;
    public bool IsPlayerOnTile() => m_isPlayerOnTurretTile;

    [Header("UI")]
    //상호작용 UI
    public GameObject InteractUiText;
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;

    //Turret
    [SerializeField] private TurretSo builtTurretSo; 
    [SerializeField] private float m_remainingBuildTime = 0f; //설치까지 남은 시간
    private bool isInstalling = false; // 현재 설치 중인지 여부
    public TurretSo GetBuiltTurret() => builtTurretSo;
    public float GetRemainingInstallTime() => m_remainingBuildTime;
    public bool IsInstalling() => isInstalling;

    private void Awake() => m_awakeTime = Time.time;

    private void OnEnable()
    {
        TurretUI.Instance.OnIsUIOpen += SetInteraction;
        var tile = GetComponent<Tile>();
        Destroy(tile);
        tileState = TileState.DefenceArea;
    }
    private void OnDisable()
    {
        TurretUI.Instance.OnIsUIOpen -= SetInteraction;
    }

    private void Update()
    {
        if (!isInstalling || builtTurretSo == null) return;

        m_remainingBuildTime -= Time.deltaTime;
        if (m_remainingBuildTime <= 0f)
        {
            m_remainingBuildTime = 0f;
            isInstalling = false;
        }
    }

    #region 상호작용 인터페이스 구현

    public override interactType GetInteractType() => interactType.PressE;

    public override bool CanInteract(ToolType toolType) =>
        m_isPlayerOnTurretTile && toolType == ToolType.Hammer;
    public override void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None) return;

        TurretUI.Instance.OpenUI();
        // TurretUI.Instance.SetTile(this);
        InteractUiText.SetActive(false);
    }
    private void SetInteraction(bool _status) => InteractUiText.SetActive(_status);

    #endregion

    //터렛 설치 메서드
    public void StartBuiltTurret(TurretSo so)
    {
        if (m_isTurretBuilt) return;
        builtTurretSo = so;
        m_remainingBuildTime = so.buildingTime;
        isInstalling = true;
        m_isTurretBuilt = true;
    }

    //터렛 설치 완료 메서드
    public void EndBuilding()
    {
        isInstalling = false;
        // builtTurretSo = null;
        m_remainingBuildTime = 0f;
    }



    #region 충돌처리

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnTurretTile = true;
            if (!TurretUI.Instance.GetActiveself())
            {
                InteractUiText.SetActive(true);
            }
            var player = other.GetComponent<Interaction>();
            player?.RegisterTrigger(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerOnTurretTile = false;
            InteractUiText.SetActive(false);
            var player = other.GetComponent<Interaction>();
            player?.ClearTrigger(this);
        }
    }

    #endregion

    public override void OnTileInteractionStay(Interaction player)
    {
    }
    public override void OnTileInteractionExit(Interaction player)
    {
    }
}