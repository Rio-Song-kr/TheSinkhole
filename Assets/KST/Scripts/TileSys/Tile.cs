using System;
using System.Collections;
using EPOOutline;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour, IToolInteractable, ITileInteractable
{
    public TileState tileState = TileState.None;
    // [SerializeField] private bool m_isDeveloping; //개척 중이면 t, 아니면 F
    // public bool IsDeveloping() => m_isDeveloping;
    public bool isDeveloping;

    public static GameObject InteractUiTextRef;
    //농사창 UI
    public static GameObject FarmUIRef;
    [SerializeField] private GameObject m_farmModelPrefab;
    [SerializeField] private GameObject m_turretModelPrefab;
    [SerializeField] private GameObject m_waterModelPrefab;

    private Outlinable m_outlinable;
    private Coroutine m_coroutine;
    //이벤트
    public event Action OnTileStateChanged;

    private void Awake()
    {
        m_outlinable = GetComponent<Outlinable>();
        m_outlinable.enabled = false;
    }
    private void SetTileState(TileState newState)
    {
        tileState = newState;
        OnTileStateChanged?.Invoke();

    }

    public virtual interactType GetInteractType() => interactType.MouseClick;
    public virtual bool CanInteract(ToolType toolType)
    {
        return tileState switch
        {
            TileState.None => toolType == ToolType.Pick,
            TileState.Frontier => toolType == ToolType.Shovel || toolType == ToolType.Hammer || toolType == ToolType.Water,
            _ => false,
        };
    }

    //타일마다 머터리얼 있어야 할 경우 변경할 수 있도록.
    //public Material farmMaterial, stoneMaterial;

    //곡괭이 -> 미개척지 -> 개척지
    //삽 -> 개척지 -> 경작지
    public virtual void OnInteract(ToolType toolType)
    {
        if (!CanInteract(toolType)) return;
        if(Interaction.Instance.IsKeyPressed())
            ExploitUI.Instance.OpenUI(this, toolType);

    }

    public void TileStateChange(ToolType toolType)
    {
        //타일 상태
        switch (tileState)
        {
            case TileState.None: // 미 개척지. 다른 행동은 불능이며, 곡괭이를 통해서만 개척지로 변경 가능.
                if (toolType == ToolType.Pick)
                {
                    SetTileState(TileState.Frontier);
                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Frontier);
                }
                break;

            case TileState.Frontier: // 개척지. 삽을 통해 경작지로 변경 가능하며, 추후 다른 건물을 짓는 것도 가능.
                if (toolType == ToolType.Shovel)
                {
                    if (m_coroutine != null) return;

                    SetTileState(TileState.ChangingState);

                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.ChangingState);

                    var newModel = Instantiate(m_farmModelPrefab, transform.parent).GetComponent<DissolveEffect>();
                    m_coroutine = StartCoroutine(ChangeTileState(toolType, newModel));
                }
                break;
            //TODO<김승태> : 해머, 물 추가해야함.
            case TileState.DefenceArea:
                break;
            case TileState.WaterArea:
                break;
        }
    }

    private IEnumerator ChangeTileState(ToolType toolType, DissolveEffect effect)
    {
        while (!effect.IsDone)
        {
            yield return null;
        }

        switch (toolType)
        {
            case ToolType.Shovel:
                SetFarmable();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Farmable);
                break;
            case ToolType.Hammer:
                break;
            case ToolType.Water:
                break;
        }
    }

    //테스트를 위해 잠시 public 메소드로 변경

    //FarmTile로 변경시 부착
    public void SetFarmable()
    {
        SetTileState(TileState.Farmable);
        
        if (!TryGetComponent<FarmTile>(out _))
        {
            var go = gameObject.AddComponent<FarmTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }

    //TurretTile로 변경시 부착
    public void SetDefenceArea()
    {
        SetTileState(TileState.DefenceArea);

        if (!TryGetComponent<TurretTile>(out _))
        {
            var go = gameObject.AddComponent<TurretTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }

    public void SetNoneGround()
    {
        SetTileState(TileState.None);
        if (TryGetComponent<FarmTile>(out var farmTile))
        {
            Destroy(farmTile);
        }
        if (TryGetComponent<TurretTile>(out var turretTile))
        {
            Destroy(turretTile);
        }
    }

    public virtual void OnTileInteractionStay(Interaction player)
    {
    }

    public virtual void OnTileInteractionExit(Interaction player)
    {
    }
}