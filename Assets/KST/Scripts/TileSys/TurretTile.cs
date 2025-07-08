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
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;

    //Turret
    [SerializeField] private Turret turretObj;
    public Turret GetTurretObject() => turretObj;
    [SerializeField] private TurretSo builtTurretSo;
    [SerializeField] private float m_remainingBuildTime = 0f; //설치까지 남은 시간
    private bool isInstalling = false; // 현재 설치 중인지 여부
    public TurretSo GetBuiltTurret() => builtTurretSo;
    public float GetRemainingInstallTime() => m_remainingBuildTime;
    public bool IsInstalling() => isInstalling;
    [SerializeField] private bool m_isTurretDeployed = false;
    public bool IsDeployed() => m_isTurretDeployed;
    public void SetDeployed(bool value) => m_isTurretDeployed = value;

    private void Awake() => m_awakeTime = Time.time;

    private void OnEnable()
    {
        TurretUI.Instance.OnIsUIOpen += SetInteraction;
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

        TurretUI.Instance.OpenUI(this);
        TurretUI.Instance.SetTile(this);
        InteractionUI.ClearInteractionUI(InteractionType.Turret);
    }
    private void SetInteraction(bool status)
    {
        if (status)
            InteractionUI.SetInteractionUI(InteractionType.Turret, true, "상호작용을 하려면 [E]를 눌러주세요.", false);
        else
            InteractionUI.ClearInteractionUI(InteractionType.Turret);
    }

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

    public override void OnTileInteractionStay(Interaction player)
    {
        if (Time.time - m_awakeTime < m_interactDelay) return;
        m_isPlayerOnTurretTile = true;

        var currentTool = player.CurrentTool;
        if (!FarmUI.Instance.GetActiveself() && currentTool == ToolType.Hammer)
            InteractionUI.SetInteractionUI(InteractionType.Turret, true, "상호작용을 하려면 [E]를 눌러주세요.", false);
        else
            InteractionUI.ClearInteractionUI(InteractionType.Turret);

        player?.RegisterTrigger(this);
    }

    public override void OnTileInteractionExit(Interaction player)
    {
        m_isPlayerOnTurretTile = false;
        InteractionUI.ClearInteractionUI(InteractionType.Turret);
        player?.ClearTrigger(this);
    }

    #endregion
}