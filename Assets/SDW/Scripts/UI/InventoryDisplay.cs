using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemDataUI m_mouseInventorySlot;

    protected InventorySystem m_inventorySystem;
    public InventorySystem InventorySystem => m_inventorySystem;

    protected Dictionary<InventorySlotUI, InventorySlot> m_slotDictionary;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => m_slotDictionary;

    public virtual void Start()
    {
    }

    public abstract void AssignSlot(InventorySystem inventory);

    protected virtual void UpdateSlot(InventorySlot inventorySlot)
    {
        foreach (var slot in m_slotDictionary)
        {
            if (slot.Value == inventorySlot)
            {
                slot.Key.UpdateUISlot(inventorySlot);
            }
        }
    }

    public void OnSlotClicked(InventorySlotUI slot)
    {
        //# 슬롯에 아이템이 있고, 마우스에 없는 경우
        if (slot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO == null)
        {
            //# Shift + 클릭 - 분할
            m_mouseInventorySlot.UpdateMouseSlot(slot.InventorySlot);
            slot.ClearSlot();
            return;
        }

        //# 슬롯에 아이템이 없고, 마우스에 아이템이 있는 경우
        if (slot.InventorySlot.ItemDataSO == null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            slot.InventorySlot.AddItem(m_mouseInventorySlot.InventorySlot);
            slot.UpdateUISlot();
            m_mouseInventorySlot.ClearSlot();
        }

        //# 슬롯에도 아이템이 있고, 슬롯에도 있는 경우
        //@ ㄴ 다른 아이템이면 Swap
        if (slot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            SwapSlots(slot);
        }


        //@ ㄴ 같은 아이템이면 합치기
        //     ㄴ 슬롯의 아이템 수량 + 마우스의 슬롯 수량 > Max 수량 => 마우스 아이템으로 대체
    }

    private void SwapSlots(InventorySlotUI slot)
    {
        var clonedSlot = new InventorySlot(
            m_mouseInventorySlot.InventorySlot.ItemDataSO, m_mouseInventorySlot.InventorySlot.ItemCount
        );
        m_mouseInventorySlot.ClearSlot();
        m_mouseInventorySlot.UpdateMouseSlot(slot.InventorySlot);
        slot.ClearSlot();
        slot.InventorySlot.AddItem(clonedSlot);
        slot.UpdateUISlot();
    }
}