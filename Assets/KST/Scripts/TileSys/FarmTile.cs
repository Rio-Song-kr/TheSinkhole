using UnityEngine;

public class FarmTile : Tile
{
    [Header("Status")]
    [SerializeField] private bool m_isPlanted;
    [SerializeField] private bool m_isPlayerOnFarmTile;
    public bool IsPlanted() => m_isPlanted;
    public bool IsPlayerOnTile() => m_isPlayerOnFarmTile;

    [Header("UI")]
    //상호작용 UI
    public float m_interactDelay = 0.5f;
    public float m_awakeTime;

    //Grwoing
    [SerializeField] private float growTimer = 0f;
    private bool isGrowing = false;
    [SerializeField] private CropDataSO growingCrop;

    public CropDataSO GetGrownCrop() => growingCrop;
    public float GetRemainingGrowTime() => growTimer;
    public bool IsGrowing() => isGrowing;

    private void Awake() => m_awakeTime = Time.time;

    private void OnEnable()
    {
        FarmUI.Instance.OnIsUIOpen += SetInteraction;
        tileState = TileState.FarmTile;
    }

    private void OnDisable()
    {
        FarmUI.Instance.OnIsUIOpen -= SetInteraction;
    }

    private void Update()
    {
        if (!isGrowing || growingCrop == null) return;

        growTimer -= Time.deltaTime;
        if (growTimer <= 0f)
        {
            growTimer = 0f;
            isGrowing = false;
        }
    }

    #region 상호작용 인터페이스 구현

    public override interactType GetInteractType() => interactType.PressE;

    public override bool CanInteract(ToolType toolType) =>
        m_isPlayerOnFarmTile && toolType == ToolType.Shovel;

    public override void OnInteract(ToolType toolType)
    {
        if (toolType == ToolType.None) return;

        if (GameManager.Instance.IsCursorLocked)
        {
            FarmUI.Instance.OpenUI(this);
            InteractionUI.ClearInteractionUI(InteractionType.Farm);
        }
    }

    private void SetInteraction(bool status)
    {
        if (status)
            InteractionUI.SetInteractionUI(InteractionType.Farm, true, "상호작용을 하려면 [E]를 눌러주세요.", false);
        else
            InteractionUI.ClearInteractionUI(InteractionType.Farm);
    }

    #endregion

    //심기 메서드
    public void StartPlanting(CropDataSO crop)
    {
        growingCrop = crop;
        growTimer = crop.growTime;
        isGrowing = true;
        m_isPlanted = true;
    }

    //수확 메서드
    public void HarvestingCrop()
    {
        m_isPlanted = false;
        growingCrop = null;
        isGrowing = false;
        growTimer = 0f;
    }

    #region 타일 Ray 상호작용

    public override void OnTileInteractionStay(Interaction player)
    {
        if (Time.time - m_awakeTime < m_interactDelay) return;
        m_isPlayerOnFarmTile = true;

        var currentTool = player.CurrentTool;
        if (!FarmUI.Instance.GetActiveself() && currentTool == ToolType.Shovel)
            InteractionUI.SetInteractionUI(InteractionType.Farm, true, "상호작용을 하려면 [E]를 눌러주세요.", false);
        else
            InteractionUI.ClearInteractionUI(InteractionType.Farm);

        player?.RegisterTrigger(this);
    }

    public override void OnTileInteractionExit(Interaction player)
    {
        m_isPlayerOnFarmTile = false;
        InteractionUI.ClearInteractionUI(InteractionType.Farm);
        player?.ClearTrigger(this);
    }

    #endregion
}