using System;
using UnityEngine;

// 복사 일자 : 25.06.26 11:40분
// 원본 파일 위치 : KST/Scripts/Interaction
// 목적 : 상호작용 기초 테스트
// 변경 사항 : Input.GetMouseDown -> Input Action시스템으로 변경.
// private인 MouseInteraction을 public으로 변경
public class InteractionTestfromKSTtoGIL : MonoBehaviour
{
    public ToolType CurrentTool = ToolType.None;
    [SerializeField] private Transform m_rayTransform; //레이 쏘는 위치(캐릭터의 카메라.)
    public float InteractionDistance = 2.3f; //적당한 거리로 세팅바람.(상호작용 가능한 적당한 레이 길이 필요.)

    //트리거로 들어온 대상에 따라 나눔
    private Iinteractable m_currentTargetTrigger; //도구 필요없는 상호작용 ex) Book.
    private IToolInteractable m_currentTargetTriggerTool; //도구 기반 상호작용 ex) FarmTile

    private void Update()
    {
        //MouseInteraction();
        KeyDownInteraction();
    }
    //좌클릭 상호작용
    public void MouseInteraction()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, InteractionDistance))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.TryGetComponent(out IToolInteractable toolInteractable))
            {
                if (toolInteractable.GetInteractType() == interactType.MouseClick &&
                    toolInteractable.CanInteract(CurrentTool))
                {
                    toolInteractable.OnInteract(CurrentTool);
                    return;
                }
            }
            //도구 필요 없는 대상일 경우
            if (hit.collider.TryGetComponent(out Iinteractable interactable))
            {
                Debug.Log("상호작용");
                if (interactable.GetInteractType() == interactType.MouseClick && interactable.CanInteract())
                    interactable.OnInteract();
            }
        }
    }

    //키다운 상호작용
    private void KeyDownInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
}