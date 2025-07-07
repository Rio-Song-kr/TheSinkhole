using UnityEngine;

public class WaterTile : Tile
{
    [Header("Status")]
    [SerializeField] private bool m_isWatered;
    [SerializeField] private bool m_isPlayerOnWaterTile;
    public bool IsPlayerOnTile() => m_isPlayerOnWaterTile;
    public bool IsWatered() => m_isWatered;
    [Header("UI")]
    //상호작용 UI
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;

    [Header("Water")]
    private bool m_isWatering = false;
    [SerializeField] private float m_waterTimer = 0f;
    [SerializeField] private float m_waterDuration = 180f;
    public bool IsWatering() => m_isWatering;
    // public float GetWaterDuration() => m_waterDuration;
    public float GetRemainingWaterTime() => m_waterTimer;

    private void Awake() => m_awakeTime = Time.time;

    private void OnEnable()
    {
        WaterUI.Instance.OnIsUIOpen += SetInteraction;
        tileState = TileState.WaterTile;
    }

    private void OnDisable()
    {
        WaterUI.Instance.OnIsUIOpen -= SetInteraction;
    }

    private void Update()
    {
        if (!m_isWatering) return;

        m_waterTimer -= Time.deltaTime;
        if (m_waterTimer <= 0f)
        {
            m_waterTimer = 0f;
            m_isWatering = false;
        }
    }

    #region 상호작용 인터페이스 구현

    public override interactType GetInteractType() => interactType.PressE;

    public override bool CanInteract(ToolType toolType) =>
        m_isPlayerOnWaterTile && toolType == ToolType.Water;

    public override void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None) return;

        if (GameManager.Instance.IsCursorLocked)
        {
            WaterUI.Instance.OpenUI(this);
            InteractionUI.ClearInteractionUI(InteractionType.Water);
        }
    }

    private void SetInteraction(bool status)
    {
        if (status)
            InteractionUI.SetInteractionUI(InteractionType.Water, true, "상호작용을 하려면 [E]를 눌러주세요.");
        else
            InteractionUI.ClearInteractionUI(InteractionType.Water);
    }

    #endregion

    //급수 메서드
    public void StartWatering()
    {
        m_waterTimer = m_waterDuration;
        m_isWatering = true;
        m_isWatered = true;
    }

    //물 급수 끝 메서드
    public void EndWatering()
    {
        m_isWatered = false;
        m_isWatering = false;
        m_waterTimer = 0f;
    }

    #region 타일 Ray 상호작용

    public override void OnTileInteractionStay(Interaction player)
    {
        if (Time.time - m_awakeTime < m_interactDelay) return;
        m_isPlayerOnWaterTile = true;

        var currentTool = player.CurrentTool;
        if (!WaterUI.Instance.GetActiveself() && currentTool == ToolType.Water)
            InteractionUI.SetInteractionUI(InteractionType.Water, true, "상호작용을 하려면 [E]를 눌러주세요.");
        else
            InteractionUI.ClearInteractionUI(InteractionType.Water);

        player?.RegisterTrigger(this);
    }

    public override void OnTileInteractionExit(Interaction player)
    {
        m_isPlayerOnWaterTile = false;
        InteractionUI.ClearInteractionUI(InteractionType.Water);
        player?.ClearTrigger(this);
    }

    #endregion
}