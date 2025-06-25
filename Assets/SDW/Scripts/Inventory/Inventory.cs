using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 컨테이너 역할
/// Player에 추가되며 Inventory 크기만큼 InventorySystem을 통해 슬롯을 생성하며 각 슬롯을 관리
/// Inventory와 관련된 모델(데이터 계층) 역할
/// </summary>
[Serializable]
public class Inventory : MonoBehaviour
{
    //# 비고 : 현재는 dynamicInventorySlot이라 사용하고 있지만, 추후 상자가 추가될 시 상자와 백팩은 변수명은 분리하여 구분
    [SerializeField] private int m_dynamicInventorySlotSize;
    [SerializeField] private int m_quickSlotSize;

    //# 하단의 Quick Slot용 Inventory System
    [SerializeField] private InventorySystem m_quickSlotInventorySystem;
    public InventorySystem QuickSlotInventorySystem => m_quickSlotInventorySystem;

    //# 화면 중앙에 표시되는 Backpack, 상자 등의 InventorySystem, 추후 상자 추가시 변수명은 구분해야 함
    [SerializeField] private InventorySystem m_dynamicInventorySystem;
    public InventorySystem DynamicInventorySystem => m_dynamicInventorySystem;

    public static Action<InventorySystem, bool> OnDynamicDisplayRequest;
    private bool _isDynamicInventoryOpen = false;

    /// <summary>
    /// 인벤토리 시스템들을 size 만큼 초기화
    /// </summary>
    private void Awake()
    {
        m_quickSlotInventorySystem = new InventorySystem(m_quickSlotSize);
        m_dynamicInventorySystem = new InventorySystem(m_dynamicInventorySlotSize);
    }

    /// <summary>
    /// 현재 B키 입력 시 Dynamic Inventory Open/Close
    /// Escape 키 입력 시 Dynamic Inventory Close
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _isDynamicInventoryOpen = !_isDynamicInventoryOpen;
            OnDynamicDisplayRequest?.Invoke(m_dynamicInventorySystem, _isDynamicInventoryOpen);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isDynamicInventoryOpen = false;
            OnDynamicDisplayRequest?.Invoke(m_dynamicInventorySystem, _isDynamicInventoryOpen);
        }
    }

    /// <summary>
    /// 마인크래프트 스타일 스마트 아이템 추가
    /// 인벤토리 타입에 관계없이 기존 스택을 우선으로 채우고, 이후 빈 슬롯에 추가
    /// </summary>
    /// <param name="itemData">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>추가되지 못한 남은 수량</returns>
    public int AddItemSmart(ItemDataSO itemData, int amount)
    {
        int remainingAmount = amount;

        //# 모든 인벤토리에서 기존 스택들을 수집하고 수량순으로 정렬
        var existingStacks = GetAllExistingStacks(itemData);

        //# 기존 스택들을 우선으로 채우기 (수량이 많은 순서대로)
        foreach (var stackInfo in existingStacks)
        {
            if (stackInfo.slot.CanAdd(remainingAmount, out int canAddAmount))
            {
                int addAmount = Mathf.Min(remainingAmount, canAddAmount);
                stackInfo.slot.AddItem(addAmount);
                remainingAmount -= addAmount;

                //@ 해당 인벤토리 시스템에 변경 알림
                stackInfo.inventorySystem.NotifySlotChanged(stackInfo.slot);

                if (remainingAmount == 0) return 0;
            }
        }

        //# 빈 슬롯에 추가 (QuickSlot 우선)
        if (remainingAmount > 0)
        {
            remainingAmount = AddToEmptySlots(itemData, remainingAmount);
        }

        return remainingAmount;
    }

    /// <summary>
    /// 모든 인벤토리에서 지정된 아이템의 기존 스택들을 수집하고 정렬
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>수량순으로 정렬된 스택 정보 리스트</returns>
    private List<StackInfo> GetAllExistingStacks(ItemDataSO itemData)
    {
        var allStacks = new List<StackInfo>();

        //# QuickSlot에서 기존 스택 찾기
        if (m_quickSlotInventorySystem.FindItemSlots(itemData, out var quickSlotStacks))
        {
            foreach (var slot in quickSlotStacks)
            {
                allStacks.Add(new StackInfo
                {
                    slot = slot,
                    inventorySystem = m_quickSlotInventorySystem,
                    currentAmount = slot.ItemCount
                });
            }
        }

        //# Dynamic에서 기존 스택 찾기
        if (m_dynamicInventorySystem.FindItemSlots(itemData, out var dynamicStacks))
        {
            foreach (var slot in dynamicStacks)
            {
                allStacks.Add(new StackInfo
                {
                    slot = slot,
                    inventorySystem = m_dynamicInventorySystem,
                    currentAmount = slot.ItemCount
                });
            }
        }

        //# 수량이 많은 순서대로 정렬
        allStacks.Sort((a, b) => b.currentAmount.CompareTo(a.currentAmount));

        return allStacks;
    }

    /// <summary>
    /// 빈 슬롯들에 아이템을 추가 (QuickSlot 우선)
    /// </summary>
    /// <param name="itemData">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>남은 수량</returns>
    private int AddToEmptySlots(ItemDataSO itemData, int amount)
    {
        int remainingAmount = amount;

        //# QuickSlot 빈 슬롯 우선
        while (remainingAmount > 0 && m_quickSlotInventorySystem.GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, itemData.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(itemData, addAmount);
            remainingAmount -= addAmount;

            m_quickSlotInventorySystem.NotifySlotChanged(emptySlot);
        }

        //# Dynamic 빈 슬롯
        while (remainingAmount > 0 && m_dynamicInventorySystem.GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, itemData.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(itemData, addAmount);
            remainingAmount -= addAmount;

            m_dynamicInventorySystem.NotifySlotChanged(emptySlot);
        }

        return remainingAmount;
    }

    /// <summary>
    /// 스택 정보를 담는 헬퍼 클래스
    /// </summary>
    private class StackInfo
    {
        public InventorySlot slot;
        public InventorySystem inventorySystem;
        public int currentAmount;
    }
}