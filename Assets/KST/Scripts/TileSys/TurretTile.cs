using UnityEngine;

public class TurretTile : Tile
{
    [Header("Status")]
    [SerializeField] private bool m_isBuild; //터렛 설치되어 있으면 true, 아니면 false
    [SerializeField] private bool m_isPlayerOnTurretTile; //터렛타일에 플레이어가 있으면 true, 없으면 false
    public bool IsBuild() => m_isBuild;
    public bool IsPlayerOnTile() => m_isPlayerOnTurretTile;

    [Header("UI")]
    //상호작용 UI
    public GameObject InteractUiText;
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;

    //Turret
    [SerializeField] private TurretSo builtTurret;
    [SerializeField] private float installTimer = 0f;
    private bool isInstalling = false;
    public TurretSo GetBuiltTurret() => builtTurret;
    public float GetRemainingInstallTime() => installTimer;
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
        if (!isInstalling || builtTurret == null) return;

        installTimer -= Time.deltaTime;
        if (installTimer <= 0f)
        {
            installTimer = 0f;
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
        builtTurret = so;
        installTimer = so.buildingTime;
        isInstalling = true;
        m_isBuild = true;
    }

    public void EndBuilding()
    {
        isInstalling = false;
        builtTurret = null;
        installTimer = 0f;
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