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
    private ViewmodelAnimationController animController;

    private Item itemData;

    public bool isAttakable { get; private set; }

    private void Awake()
    {
        inventory = GetComponentInParent<Inventory>();
        animController = GetComponent<ViewmodelAnimationController>();
        playerInput = new PlayerInput();
        isAttakable = false;
        onFoot = playerInput.OnFoot;
        onFoot.InventoryNumpad.started += ShowQuickslotViewModel;
        onFoot.LMBClick.started += ctx => ShowAttackAnimation();
    }

    [Header("Quickslot")]
    public Transform ItemShowPos;

    [Header("TODO : Add Item Models")]
    [SerializeField]
    private GameObject[] toolModels;

    // 선택된 것을 제외하고 전부 비활성화하기. 이전에 것을 기억하는 것도 좋지만 스위치 형식을 사용하기엔 너무 복잡할듯
    private void ActivateSelectedToolOnly(int index)
    {
        foreach (var tool in toolModels)
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
        try
        {
            itemData = inventory.QuickSlotInventorySystem.InventorySlots[selectedIndex].ItemDataSO.ItemData;
        }
        catch (NullReferenceException n)
        {
            ActivateSelectedToolOnly(8);
            isAttakable = false;
            return;
        }
        // 데이터 테이블 순서대로 배치(혼동 방지)
        switch (itemData.ItemId)
        {
            // 물통
            case 20104:
                ActivateSelectedToolOnly(0);
                isAttakable = false;
                break;
            // 붕대
            case 20301:
                ActivateSelectedToolOnly(1);
                isAttakable = false;
                break;
            // 감자
            case 20901:
                ActivateSelectedToolOnly(2);
                isAttakable = false;
                break;
            // 삽
            case 20303:
                ActivateSelectedToolOnly(3);
                isAttakable = true;
                break;
            // 망치
            case 20304:
                ActivateSelectedToolOnly(4);
                isAttakable = true;
                break;
            // 담배
            case 20305:
                ActivateSelectedToolOnly(5);
                isAttakable = false;
                break;
            // 곡괭이
            case 20307:
                ActivateSelectedToolOnly(6);
                isAttakable = true;
                break;
            // 양동이
            case 20308:
                ActivateSelectedToolOnly(7);
                isAttakable = false;
                break;
            // 플레어 건
            case 20302:
                ActivateSelectedToolOnly(8);
                isAttakable = false;
                break;
            // 그 외
            default:
                ActivateSelectedToolOnly(9);
                isAttakable = false;
                break;
        }
    }

    private void ShowAttackAnimation()
    {
        if (isAttakable == false) return;
        if (!GameManager.Instance.IsCursorLocked) return;
        animController.SetAttack();
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