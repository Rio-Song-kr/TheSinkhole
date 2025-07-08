using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 인벤토리 로직 및 데이터 관리
/// Inventory와 관련된 모델(데이터 계층) 역할
/// </summary>
[Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> m_inventorySlots;
    public List<InventorySlot> InventorySlots => m_inventorySlots;
    public int InventorySize => m_inventorySlots.Count;

    public UnityAction<InventorySlot> OnSlotChanged;

    /// <summary>
    /// 초기 InventorySystem 생성 시 size 만큼 생성
    /// </summary>
    /// <param name="size">인벤토리 슬롯 수</param>
    public InventorySystem(int size)
    {
        m_inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            m_inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// 인벤토리에 아이템을 지정한 수만큼 추가
    /// 동일한 이름의 아이템이 이미 인벤토리에 존재할 경우, 아이템 수량 증가
    /// 동일한 이름의 아이템이 있지만, 수량을 더했을 때 MaxItemCount를 넘어선다면 새 슬롯에 추가
    /// 추가하려는 아이템이 인벤토리에 없거나, 위의 수량 문제가 있을 경우 새 슬롯에 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>남은 amount를 반환</returns>
    public int AddItem(ItemDataSO item, int amount)
    {
        int remainingAmount = amount;

        if (FindItemSlots(item, out var existingSlots))
        {
            foreach (var slot in existingSlots)
            {
                if (slot.CanAdd(remainingAmount, out int canAddAmount))
                {
                    int addAmount = Mathf.Min(remainingAmount, canAddAmount);
                    slot.AddItem(addAmount);
                    remainingAmount -= addAmount;

                    OnSlotChanged?.Invoke(slot);

                    if (remainingAmount == 0) return 0;
                }
            }
        }

        while (remainingAmount > 0 && GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, item.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(item, addAmount);
            remainingAmount -= addAmount;

            OnSlotChanged?.Invoke(emptySlot);
        }

        return remainingAmount;
    }

    /// <summary>
    /// 추가하려는 아이템이 슬롯에 있는 아이템인지 확인
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="inventorySlot">해당 아이템이 있을 때 각 슬롯을 저장할 리스트</param>
    /// <returns>추가하려는 아이템이 슬롯에 있으면 true, 없으면 false</returns>
    public bool FindItemSlots(ItemDataSO item, out List<InventorySlot> inventorySlot)
    {
        inventorySlot = new List<InventorySlot>();

        foreach (var slot in m_inventorySlots)
        {
            if (slot.ItemDataSO == null) continue;
            if (slot.ItemDataSO.ItemEnName == item.ItemEnName) inventorySlot.Add(slot);
        }

        return inventorySlot.Count > 0;
    }

    /// <summary>
    /// 빈 슬롯을 찾아서 반환
    /// </summary>
    /// <param name="emptySlot">찾은 빈 슬롯</param>
    /// <returns>빈 슬롯이 있으면 true</returns>
    public bool GetEmptySlot(out InventorySlot emptySlot)
    {
        foreach (var slot in m_inventorySlots)
        {
            if (slot.ItemDataSO == null)
            {
                emptySlot = slot;
                return true;
            }
        }
        emptySlot = null;
        return false;
    }

    /// <summary>
    /// 인벤토리에 아이템을 스마트하게 추가 (기존 스택 우선)
    /// 동일한 아이템의 기존 슬롯을 먼저 채우고, 이후 빈 슬롯에 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>남은 amount를 반환</returns>
    public int AddItemSmart(ItemDataSO item, int amount)
    {
        int remainingAmount = amount;

        //#기존 슬롯들을 먼저 채우기
        if (FindItemSlots(item, out var existingSlots))
        {
            foreach (var slot in existingSlots)
            {
                if (slot.CanAdd(remainingAmount, out int canAddAmount))
                {
                    int addAmount = Mathf.Min(remainingAmount, canAddAmount);
                    slot.AddItem(addAmount);
                    remainingAmount -= addAmount;

                    OnSlotChanged?.Invoke(slot);

                    if (remainingAmount == 0) return 0;
                }
            }
        }

        //# 빈 슬롯에 추가
        while (remainingAmount > 0 && GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, item.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(item, addAmount);
            remainingAmount -= addAmount;

            OnSlotChanged?.Invoke(emptySlot);
        }

        return remainingAmount;
    }

    /// <summary>
    /// 인벤토리가 아이템을 수용할 수 있는 총 공간을 계산
    /// </summary>
    /// <param name="item">확인할 아이템</param>
    /// <returns>수용 가능한 총 개수</returns>
    public int GetAvailableSpace(ItemDataSO item)
    {
        int totalSpace = 0;

        //# 기존 스택들의 여유 공간 계산
        if (FindItemSlots(item, out var existingSlots))
        {
            foreach (var slot in existingSlots)
            {
                if (slot.CanAdd(1, out int canAddAmount)) totalSpace += canAddAmount;
            }
        }

        //# 빈 슬롯들의 공간 계산
        foreach (var slot in m_inventorySlots)
        {
            if (slot.ItemDataSO.Equals(null)) totalSpace += item.ItemMaxOwn;
        }

        return totalSpace;
    }

    /// <summary>
    /// 인벤토리에 빈 슬롯이 있는지 확인
    /// </summary>
    /// <returns>빈 슬롯이 있으면 true</returns>
    public bool HasEmptySlot() => GetEmptySlot(out _);

    /// <summary>
    /// 슬롯 변경 알림을 외부에서 호출할 수 있도록 하는 메서드
    /// </summary>
    /// <param name="slot">변경된 슬롯</param>
    public void NotifySlotChanged(InventorySlot slot) => OnSlotChanged?.Invoke(slot);
}