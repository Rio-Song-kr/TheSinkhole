using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewmodelManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    [Header("Quickslot")]
    public Transform ItemShowPos;


    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onFoot.InventoryNumpad.started += ShowQuickslotViewModel;
        Debug.Log("Viewmodel 입력 완료");
    }


    /// <summary>
    /// 퀵슬롯에 있는 아이템의 3D 모델링을 오른손에 보여주기
    /// </summary>
    public void ShowQuickslotViewModel(InputAction.CallbackContext ctx)
    {
        //if(퀵슬롯 번호에 아이템이 있냐?)
        //{
        //  있을 경우 해당 아이템의 프리팹 정보를 받아오기
        //  프리팹을 ItemShowPos에 배치하기
        //  프리팹에 콜라이더가 있을 경우 비활성화하기(충돌 방지)    
        //}
        Debug.Log(ctx.control.name);
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
