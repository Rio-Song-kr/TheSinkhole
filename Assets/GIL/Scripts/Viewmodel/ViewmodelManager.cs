using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class ViewmodelManager : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private Item itemData;


    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onFoot.InventoryNumpad.started += ShowQuickslotViewModel;
    }

    [Header("Quickslot")]
    public Transform ItemShowPos;

    [Header("TODO : Add Item Models")]
    [SerializeField] GameObject[] toolModels;

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
        int selectedIndex = int.Parse(ctx.control.name);

        selectedIndex = selectedIndex == 0 ? 9 : selectedIndex - 1;
        Debug.Log($"현재 번호 {ctx.control.name}");
        try
        {
            itemData = inventory.QuickSlotInventorySystem.InventorySlots[selectedIndex].ItemDataSO.ItemData;
        }
        catch (NullReferenceException n)
        {
            Debug.Log("빈 공간");
            ActivateSelectedToolOnly(3);
            return;
        }
        switch (itemData.ItemId)
        {
            case 20303:
                ActivateSelectedToolOnly(0);
                Debug.Log("삽");
                break;
            case 20304:
                ActivateSelectedToolOnly(1);
                Debug.Log("망치");
                break;
            case 20307:
                ActivateSelectedToolOnly(2);
                Debug.Log("곡괭이");
                break;
            default:
                Debug.LogWarning("없는 id입니다!");
                break;
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
