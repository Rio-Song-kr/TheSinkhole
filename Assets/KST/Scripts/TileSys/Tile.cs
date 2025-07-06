using System;
using System.Collections;
using UnityEngine;
using EPOOutline;

public class Tile : MonoBehaviour, IToolInteractable, ITileInteractable
{
    public TileState tileState = TileState.PlainTile;
    public static GameObject InteractUiTextRef;
    //농사창 UI
    public static GameObject FarmUIRef;

    [SerializeField] private GameObject m_farmModelPrefab;
    [SerializeField] private GameObject m_turretModelPrefab;
    [SerializeField] private GameObject m_waterModelPrefab;

    private Outlinable m_outlinable;
    private Coroutine m_coroutine;
    // 개척 관련 변수
    public DevelopSO developSO;
    public bool isDeveloping => m_isDeveloping;
    private float m_developDuration;
    private float m_developStartTime;
    private Coroutine m_developCoroutine;
    private bool m_isDeveloping;

    //이벤트
    public event Action OnTileStateChanged;

    private void Awake()
    {
        m_outlinable = GetComponent<Outlinable>();
        m_outlinable.enabled = false;
    }
    public void SetDevelopSO(DevelopSO so)
    {
        developSO = so;
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
            //타일이 미개척지일 때는 삽으로만 상호작용해서 개척 UI를,
            TileState.PlainTile => toolType == ToolType.Pick,
            //타일이 개척지일 때는 삽,망치,물컵으로 개척 UI를 띄울 수 있음. 
            TileState.Frontier => toolType == ToolType.Shovel || toolType == ToolType.Hammer || toolType == ToolType.Water,
            _ => false,
        };
    }

    public virtual void OnInteract(ToolType toolType)
    {
        if (!CanInteract(toolType)) return;
        if (Interaction.Instance.IsKeyPressed())
            if (!ExploitUI.Instance.IsOpen)
                ExploitUI.Instance.OpenUI(this, toolType);
    }

    public void StartDevelop(float duration, Action onComplete)
    {
        if (m_developCoroutine != null) return;

        m_developDuration = duration;
        m_developStartTime = Time.time;
        m_developCoroutine = StartCoroutine(IE_Develop(onComplete));
        m_isDeveloping = true;

        OnTileStateChanged?.Invoke();
    }

    private IEnumerator IE_Develop(Action onComplete)
    {
        yield return new WaitForSeconds(m_developDuration);
        m_isDeveloping = false;
        m_developCoroutine = null;
        onComplete?.Invoke();
        OnTileStateChanged?.Invoke();
    }

    public float GetRemainingDevelopTime()
    {
        if (!m_isDeveloping) return 0f;
        float elapsed = Time.time - m_developStartTime;
        return Mathf.Max(0f, m_developDuration - elapsed);
    }

    public void TileStateChange(ToolType toolType)
    {
        //타일 상태
        switch (tileState)
        {
            case TileState.PlainTile: // 미 개척지. 다른 행동은 불능이며, 곡괭이를 통해서만 개척지로 변경 가능.
                if (toolType == ToolType.Pick)
                {
                    SetTileState(TileState.Frontier);
                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Frontier);
                }
                break;

            case TileState.Frontier: // 개척지. 삽을 통해 경작지로 변경 가능하며, 추후 다른 건물을 짓는 것도 가능.
                if (toolType == ToolType.Shovel)
                {
                    // if (m_coroutine != null) return;

                    // SetTileState(TileState.ChangingState);

                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.ChangingState);

                    // var newModel = Instantiate(m_farmModelPrefab, transform.parent).GetComponent<DissolveEffect>();
                    // m_coroutine = StartCoroutine(ChangeTileState(toolType, newModel));
                    ChangeTileState(toolType);
                }
                else if (toolType == ToolType.Hammer)
                {
                    // if (m_coroutine != null) return;

                    SetTileState(TileState.ChangingState);

                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.ChangingState);

                    // var newModel = Instantiate(m_farmModelPrefab, transform.parent).GetComponent<DissolveEffect>();
                    // m_coroutine = StartCoroutine(ChangeTileState(toolType, newModel));
                    ChangeTileState(toolType);
                }
                else if (toolType == ToolType.Water)
                {
                    // if (m_coroutine != null) return;

                    SetTileState(TileState.ChangingState);

                    GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.ChangingState);

                    // var newModel = Instantiate(m_farmModelPrefab, transform.parent).GetComponent<DissolveEffect>();
                    // m_coroutine = StartCoroutine(ChangeTileState(toolType, newModel));
                    ChangeTileState(toolType);
                }
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
                SetDefenceArea();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.DefenceArea);
                break;
            case ToolType.Water:
                SetWaterArea();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Water);
                break;
        }
    }
    private void ChangeTileState(ToolType toolType)
    {
        switch (toolType)
        {
            case ToolType.Shovel:
                SetFarmable();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Farmable);
                break;
            case ToolType.Hammer:
                SetDefenceArea();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.DefenceArea);
                break;
            case ToolType.Water:
                SetWaterArea();
                GameManager.Instance.UI.Popup.DisplayPopupView(PopupType.Water);
                break;
        }
    }
    //테스트를 위해 잠시 public 메소드로 변경

    //FarmTile로 변경시 부착
    public void SetFarmable()
    {
        SetTileState(TileState.FarmTile);
        if (!TryGetComponent<FarmTile>(out _))
        {
            var go = gameObject.AddComponent<FarmTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }
    //TurretTile로 변경시 부착
    public void SetDefenceArea()
    {
        Debug.Log("방어타일로 변경");
        SetTileState(TileState.DefenceArea);
        if (!TryGetComponent<TurretTile>(out _))
        {
            var go = gameObject.AddComponent<TurretTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }
    public void SetWaterArea()
    {
        SetTileState(TileState.WaterTile);
        if (!TryGetComponent<WaterTile>(out _))
        {
            var go = gameObject.AddComponent<WaterTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }

    public void SetNoneGround()
    {
        SetTileState(TileState.PlainTile);
        if (TryGetComponent<FarmTile>(out var farmTile))
            Destroy(farmTile);
        if (TryGetComponent<TurretTile>(out var turretTile))
            Destroy(turretTile);
    }

    public virtual void OnTileInteractionStay(Interaction player) { }

    public virtual void OnTileInteractionExit(Interaction player) { }
}