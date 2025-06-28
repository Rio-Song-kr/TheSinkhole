using System;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public ToolType CurrentTool = ToolType.None;
    [SerializeField] private Transform m_rayTransform; //레이 쏘는 위치(캐릭터의 카메라.)
    [SerializeField] private GameObject m_crosshairObject;
    public float InteractionDistance = 2.3f; //적당한 거리로 세팅바람.(상호작용 가능한 적당한 레이 길이 필요.)

    //트리거로 들어온 대상에 따라 나눔
    private Iinteractable m_currentTargetTrigger; //도구 필요없는 상호작용 ex) Book.
    private IToolInteractable m_currentTargetTriggerTool; //도구 기반 상호작용 ex) FarmTile

    private bool m_isMouseButtonClicked;
    private bool m_isIneractionKeyPressed;

    [SerializeField] private Inventory m_inventory;
    [SerializeField] private GameObject m_itemPickUpTextObject;
    [SerializeField] private TextMeshProUGUI m_itemPickUpText;
    [NonSerialized] public RaycastHit Hit;
    [NonSerialized] public bool IsDetected;

    private void Awake() => m_inventory = GetComponent<Inventory>();

    private void Update()
    {
        IsDetected = GetRaycastHit();
        MouseInteraction();
        KeyDownInteraction();
    }
    //좌클릭 상호작용
    private void MouseInteraction()
    {
        //todo 낮, 밤 관련된 스크립트 추가 - 낮이면 Tile, 밤이면 공격
        // if (!m_isMouseButtonClicked) return;

        if (GameManager.Instance.IsDay) DayAction(); // 타일 개척
        else NightAction(); // Attack
    }
    private void DayAction()
    {
        CurrentTool = m_inventory.GetItemToolType();

        if (CurrentTool == ToolType.None) return;

        if (IsDetected)
        {
            if (Hit.collider.TryGetComponent(out IToolInteractable toolInteractable))
            {
                var tileState = Hit.collider.GetComponent<Tile>().tileState;
                if (CurrentTool == ToolType.Pick && tileState == TileState.None) SetTextObject(true, "개척하려면 [E] 키를 눌러주세요.");

                else if (tileState != TileState.None)
                {
                    switch (CurrentTool)
                    {
                        case ToolType.Hammer:
                            SetTextObject(true, "방어시설 타일로 변환하려면 [E] 키를 눌러주세요.");
                            break;
                        case ToolType.Shovel:
                            SetTextObject(true, "경작지 타일로 변환하려면 [E] 키를 눌러주세요.");
                            break;
                        case ToolType.Water:
                            SetTextObject(true, "급수시설 타일로 변환하려면 [E] 키를 눌러주세요.");
                            break;
                    }
                }
                else
                    SetTextObject(false);

                SetCrosshairObject(false);

                if (toolInteractable.GetInteractType() == interactType.MouseClick &&
                    toolInteractable.CanInteract(CurrentTool) && m_isIneractionKeyPressed)
                {
                    toolInteractable.OnInteract(CurrentTool);
                    return;
                }
            }
            //도구 필요 없는 대상일 경우
            if (Hit.collider.TryGetComponent(out Iinteractable interactable))
            {
                if (interactable.GetInteractType() == interactType.MouseClick && interactable.CanInteract())
                    interactable.OnInteract();
            }
        }
        else
        {
            SetTextObject(false);

            if (GameManager.Instance.IsCursorLocked)
                SetCrosshairObject(true);
        }
    }

    private bool GetRaycastHit()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out Hit, InteractionDistance);
    }

    //todo 밤에는 공격 - 구현
    private void NightAction()
    {
    }

    //키다운 상호작용
    private void KeyDownInteraction()
    {
        if (m_isIneractionKeyPressed)
        {
            //도구 상호작용
            if (m_currentTargetTriggerTool != null &&
                m_currentTargetTriggerTool.GetInteractType() == interactType.PressE &&
                m_currentTargetTriggerTool.CanInteract(CurrentTool))
            {
                m_currentTargetTriggerTool.OnInteract(CurrentTool);
                return;
            }
            //일반 상호작용
            if (m_currentTargetTrigger != null &&
                m_currentTargetTrigger.GetInteractType() == interactType.PressE &&
                m_currentTargetTrigger.CanInteract())
            {
                m_currentTargetTrigger.OnInteract();
            }
        }
    }

    public void SetCurrentTool(ToolType toolType)
    {
        CurrentTool = toolType;
    }
    //트리거 진입 시 등록
    public void RegisterTrigger(Iinteractable iinteractable)
    {
        m_currentTargetTrigger = iinteractable;
    }
    //해제
    public void ClearTrigger(Iinteractable iinteractable)
    {
        if (m_currentTargetTrigger == iinteractable)
            m_currentTargetTrigger = null;
    }

    //도구 상호작용 트리거 진입 시 등록
    public void RegisterTrigger(IToolInteractable iinteractable)
    {
        m_currentTargetTriggerTool = iinteractable;
    }
    //해제
    public void ClearTrigger(IToolInteractable iinteractable)
    {
        if (m_currentTargetTriggerTool == iinteractable)
            m_currentTargetTriggerTool = null;
    }

    public void SetTextObject(bool isActive, string text = "")
    {
        m_itemPickUpTextObject.SetActive(isActive);
        m_itemPickUpText.text = text;
    }

    public void SetCrosshairObject(bool isActive) => m_crosshairObject.SetActive(isActive);

    public void OnMouseButtonPressed() => m_isMouseButtonClicked = true;
    public void OnMouseButtonReleased() => m_isMouseButtonClicked = false;
    public void OnInteractionKeyPressed() => m_isIneractionKeyPressed = true;
    public void OnInteractionKeyReleased() => m_isIneractionKeyPressed = false;
}