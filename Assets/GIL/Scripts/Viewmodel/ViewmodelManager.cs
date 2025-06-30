using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewmodelManager : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    [Header("Quickslot")]
    public Transform ItemShowPos;

    [Header("TODO : Add Item Models")]
    [SerializeField] GameObject[] toolModels;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onFoot.InventoryNumpad.started += ShowQuickslotViewModel;
    }


    // 선택된 것을 제외하고 전부 비활성화하기. 이전에 것을 기억하는 것도 좋지만 스위치 형식을 사용하기엔 너무 복잡할듯
    private void ActivateSelectedToolOnly(int index)
    {
        foreach (GameObject tool in toolModels)
        {
            tool.SetActive(false);
        }
        toolModels[index].SetActive(true);
    }

    /// <summary>
    /// 퀵슬롯에 있는 아이템의 3D 모델링을 오른손에 보여주기
    /// </summary>
    // 우선 모델링이 정해지지 않았기 때문에 퀵슬롯에 아이템이 있을 경우 해당 아이템 이름만 받아오는 방식으로 구현
    // TODO : 모델링이 정해지면 해당 프리팹을 보여주기
    public void ShowQuickslotViewModel(InputAction.CallbackContext ctx)
    {
        inventory.OnNumpadKeyPressed(ctx);
        Debug.Log(inventory.GetItemToolType());
        ToolType tooltype = inventory.GetItemToolType();
        switch (tooltype)
            {
                case ToolType.None:
                    ActivateSelectedToolOnly(3);
                    Debug.Log("아무것도 안 보여주기");
                    break;
                case ToolType.Shovel:
                    ActivateSelectedToolOnly(0);
                    Debug.Log("삽 아이템 보여주기");
                    break;
                case ToolType.Hammer:
                    ActivateSelectedToolOnly(1);
                    Debug.Log("망치 아이템 보여주기");
                    break;
                case ToolType.Pick:
                    ActivateSelectedToolOnly(2);
                    Debug.Log("곡괭이 아이템 보여주기");
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
