using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> _inventorySlots;
    public List<InventorySlot> InventorySlots => _inventorySlots;
    public int InventorySize => _inventorySlots.Count;

    // public UnityAction<InventorySlot> OnSlotChanged;

    /// <summary>
    /// 초기 InventorySystem 생성 시 size 만큼 생성
    /// </summary>
    /// <param name="size">인벤토리 슬롯 수</param>
    public InventorySystem(int size)
    {
        _inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            _inventorySlots.Add(new InventorySlot());
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
    public int AddItem(InventoryItemDataSO item, int amount)
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

                    // OnInventorySlotChanged?.Invoke(slot);
                    if (remainingAmount == 0) return 0;
                }
            }
        }

        while (remainingAmount > 0 && GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, item.MaxItemCount);
            emptySlot.AddItemToEmptySlot(item, addAmount);
            remainingAmount -= addAmount;

            // OnInventorySlotChanged?.Invoke(emptySlot);
        }

        return remainingAmount;
    }

    /// <summary>
    /// 추가하려는 아이템이 슬롯에 있는 아이템인지 확인
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="inventorySlot">해당 아이템이 있을 때 각 슬롯을 저장할 리스트</param>
    /// <returns></returns>
    public bool FindItemSlots(InventoryItemDataSO item, out List<InventorySlot> inventorySlot)
    {
        inventorySlot = new List<InventorySlot>();

        foreach (var slot in _inventorySlots)
        {
            if (slot.ItemData == item) inventorySlot.Add(slot);
        }

        // return inventorySlot != null;
        return inventorySlot.Count > 0;
    }

    /// <summary>
    /// 빈 슬롯이 있는지 확인
    /// </summary>
    /// <param name="emptySlot">빈 슬롯이 있을 경우, 해당 슬롯 그렇지 않을 경우 null을 가짐</param>
    /// <returns>빈 슬롯이 있을 때는 true, 없을 때는 false를 반환</returns>
    public bool GetEmptySlot(out InventorySlot emptySlot)
    {
        emptySlot = null;

        foreach (var slot in _inventorySlots)
        {
            if (slot.ItemData == null)
            {
                emptySlot = slot;
                break;
            }
        }

        return emptySlot != null;
    }
}