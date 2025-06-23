using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 인벤토리에 아이템을 표시하기 위한 추상 클래스
/// 현재는 Quick Slot만 구현되었으나 추구 플레이어의 인벤토리 구현 시 상속 예정
/// </summary>
public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemDataUI m_mouseInventorySlot;

    protected InventorySystem m_inventorySystem;
    public InventorySystem InventorySystem => m_inventorySystem;

    protected Dictionary<InventorySlotUI, InventorySlot> m_slotDictionary;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => m_slotDictionary;

    private InventorySlotUI m_originalSlot;

    public virtual void Start()
    {
    }

    public abstract void AssignSlot(InventorySystem inventory);

    /// <summary>
    /// Slot의 변동 사항이 있을 때 업데이트를 위해 사용하는 메서드
    /// Slot의 내용이 변경될 때 UI 또한 같이 업데이트
    /// </summary>
    /// <param name="inventorySlot"></param>
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

    /// <summary>
    /// 인벤토리와 마우스의 상호작용을 구현한 메서드
    /// 아이템 들기, 버리기, 이동, 스왑, 분할 기능
    /// </summary>
    /// <param name="slot">마우스가 클릭된 슬롯</param>
    public void OnSlotClicked(InventorySlotUI slot)
    {
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);

        //# 슬롯에 아이템이 있고, 마우스에 없는 경우
        if (slot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO == null)
        {
            //todo 추후 아이템 설명창으로 변경
            //# Shift + 클릭 - 분할
            if (shiftPressed && slot.InventorySlot.SplitItemAmount(out var halfAmountSlot))
            {
                m_mouseInventorySlot.UpdateMouseSlot(halfAmountSlot);
                slot.UpdateUISlot();
                return;
            }
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

        //# 슬롯에도 아이템이 있고, 마우스에도 있는 경우
        if (slot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            //@ 다른 아이템이면 Swap
            if (slot.InventorySlot.ItemDataSO != m_mouseInventorySlot.InventorySlot.ItemDataSO)
            {
                SwapSlots(slot);
                return;
            }

            //@ 같은 아이템이면 합치기 - 해당 슬롯에 아이템 수량만큼 추가되어도 ItemMaxOwn보다 작거나 같은 경우
            if (slot.InventorySlot.CanAdd(m_mouseInventorySlot.InventorySlot.ItemCount))
            {
                slot.InventorySlot.AddItem(m_mouseInventorySlot.InventorySlot);
                slot.UpdateUISlot();
                m_mouseInventorySlot.ClearSlot();
                return;
            }

            //@ 같은 아이템이면 합치기 - 해당 슬롯에 아이템 수량만큼 추가되면 ItemMaxOwn보다 커지는 경우
            slot.InventorySlot.CanAdd(m_mouseInventorySlot.InventorySlot.ItemCount, out int remaining);

            if (remaining < 1)
            {
                SwapSlots(slot);
                return;
            }

            int remainingOnMouse = m_mouseInventorySlot.InventorySlot.ItemCount - remaining;
            slot.InventorySlot.AddItem(remaining);
            slot.UpdateUISlot();

            var newItem = new InventorySlot(m_mouseInventorySlot.InventorySlot.ItemDataSO, remainingOnMouse);
            m_mouseInventorySlot.ClearSlot();
            m_mouseInventorySlot.UpdateMouseSlot(newItem);
        }
    }

    /// <summary>
    /// 마우스의 슬롯과 Inventory의 슬롯을 Swap하기 위한 메서드
    /// </summary>
    /// <param name="slot">마우스의 slot</param>
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

    public void OnBeginDragSlot(InventorySlotUI slot, PointerEventData eventData)
    {
        m_originalSlot = slot; // 드래그 시작 시 원래 슬롯 저장

        // 드래그 시작 시 슬롯에 아이템이 있으면 마우스에 할당
        if (slot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO == null)
        {
            bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
            if (shiftPressed && slot.InventorySlot.SplitItemAmount(out var halfAmountSlot))
            {
                m_mouseInventorySlot.UpdateMouseSlot(halfAmountSlot);
                slot.UpdateUISlot();
            }
            else
            {
                m_mouseInventorySlot.UpdateMouseSlot(slot.InventorySlot);
                slot.ClearSlot();
            }
        }
    }

    public void OnDragSlot(InventorySlotUI slot, PointerEventData eventData)
    {
        // 마우스 커서에 따라 아이템 UI 위치 업데이트
        if (m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            m_mouseInventorySlot.transform.position = eventData.position;
        }
    }

    public void OnEndDragSlot(InventorySlotUI slot, PointerEventData eventData)
    {
        // Quick Slot의 RectTransform 영역 확인
        var canvasRect = GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, null, out localPoint);

        if (!RectTransformUtility.RectangleContainsScreenPoint(canvasRect, eventData.position))
        {
            // Quick Slot 영역 밖이면 아이템 버리기
            if (m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
            {
                m_mouseInventorySlot.ClearSlot();
            }
            return;
        }

        // Quick Slot 영역 내에서 드롭 대상 슬롯 찾기
        var targetSlot = GetDropTarget(eventData);

        if (targetSlot == null)
        {
            // 슬롯이 아닌 경우 (슬롯 사이 공간), 원래 슬롯으로 되돌리기
            if (m_originalSlot != null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
            {
                m_originalSlot.InventorySlot.AddItem(m_mouseInventorySlot.InventorySlot);
                m_originalSlot.UpdateUISlot();
                m_mouseInventorySlot.ClearSlot();
                m_originalSlot = null; // 원래 슬롯 초기화
            }
            return;
        }

        // 드롭 대상 슬롯에 아이템 처리
        if (targetSlot.InventorySlot.ItemDataSO == null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            targetSlot.InventorySlot.AddItem(m_mouseInventorySlot.InventorySlot);
            targetSlot.UpdateUISlot();
            m_mouseInventorySlot.ClearSlot();
        }
        else if (targetSlot.InventorySlot.ItemDataSO != null && m_mouseInventorySlot.InventorySlot.ItemDataSO != null)
        {
            if (targetSlot.InventorySlot.ItemDataSO != m_mouseInventorySlot.InventorySlot.ItemDataSO)
            {
                SwapSlots(targetSlot);
            }
            else
            {
                if (targetSlot.InventorySlot.CanAdd(m_mouseInventorySlot.InventorySlot.ItemCount))
                {
                    targetSlot.InventorySlot.AddItem(m_mouseInventorySlot.InventorySlot);
                    targetSlot.UpdateUISlot();
                    m_mouseInventorySlot.ClearSlot();
                }
                else
                {
                    targetSlot.InventorySlot.CanAdd(m_mouseInventorySlot.InventorySlot.ItemCount, out int remaining);
                    if (remaining < 1)
                    {
                        SwapSlots(targetSlot);
                    }
                    else
                    {
                        int remainingOnMouse = m_mouseInventorySlot.InventorySlot.ItemCount - remaining;
                        targetSlot.InventorySlot.AddItem(remaining);
                        targetSlot.UpdateUISlot();

                        var newItem = new InventorySlot(m_mouseInventorySlot.InventorySlot.ItemDataSO, remainingOnMouse);
                        m_mouseInventorySlot.ClearSlot();
                        m_mouseInventorySlot.UpdateMouseSlot(newItem);
                    }
                }
            }
        }
    }

    private InventorySlotUI GetDropTarget(PointerEventData eventData)
    {
        foreach (var go in eventData.hovered)
        {
            var slotUI = go.GetComponent<InventorySlotUI>();
            if (slotUI != null && slotUI.ParentDisplay == this)
            {
                return slotUI;
            }
        }
        return null;
    }
}