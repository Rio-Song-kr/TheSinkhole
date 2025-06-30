using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewmodelManager : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    [Header("Quickslot")]
    public Transform ItemShowPos;


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onFoot.InventoryNumpad.started += ShowQuickslotViewModel;
    }


    /// <summary>
    /// 퀵슬롯에 있는 아이템의 3D 모델링을 오른손에 보여주기
    /// </summary>
    // 우선 모델링이 정해지지 않았기 때문에 퀵슬롯에 아이템이 있을 경우 해당 아이템 이름만 받아오는 방식으로 구현
    // TODO : 모델링이 정해지면 해당 프리팹을 보여주기
    public void ShowQuickslotViewModel(InputAction.CallbackContext ctx)
    {
        ToolType tooltype = inventory.GetItemToolType();
        inventory.OnNumpadKeyPressed(ctx);
        switch (tooltype)
        {
            case ToolType.None:
                Debug.Log("일반 아이템, 보여줄 게 없다.");
                break;
            case ToolType.Shovel:
                Debug.Log("삽 아이템 보여주기");
                break;
            case ToolType.Hammer:
                Debug.Log("망치 아이템 보여주기");
                break;
            case ToolType.Pick:
                Debug.Log("곡괭이 아이템 보여주기");
                break;
            default:
                Debug.LogWarning("Quick Slot 부분에 뭔가 문제가 발생했다.");
                break;
        }
        //inventory.GetItemName();
        //if(퀵슬롯 번호에 아이템이 있냐?)
        //{
        //  있을 경우 해당 아이템의 프리팹 정보를 받아오기
        //  프리팹을 ItemShowPos에 배치하기
        //  프리팹에 콜라이더가 있을 경우 비활성화하기(충돌 방지)    
        //}
    }

    private void ShowToolItems(ToolType toolType)
    {
        
    }


    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisalbe()
    {
        onFoot.Disable();
    }
}
