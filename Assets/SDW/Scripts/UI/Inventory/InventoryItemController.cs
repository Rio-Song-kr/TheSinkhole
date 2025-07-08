using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 아이템의 핵심 로직을 처리하는 컨트롤러 클래스
/// 아이템 픽업, 배치, 교환, 스택 등의 모든 아이템 조작 기능을 담당
/// </summary>
public class InventoryItemController
{
    private Dictionary<int, InventorySlot> m_slotMapping;
    private IMouseItemView m_mouseItemView;
    private System.Action<int> m_onSlotUpdated;

    // 인벤토리 타입을 구분하기 위한 추가 필드
    private InventorySystem m_inventorySystem;
    private System.Action<ItemDataSO, int> m_onItemInfoRequested; // 아이템 정보 표시 요청 콜백

    /// <summary>
    /// InventoryItemController 생성자
    /// </summary>
    /// <param name="slotMapping">슬롯 인덱스와 InventorySlot의 매핑 딕셔너리</param>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    /// <param name="onSlotUpdated">슬롯 업데이트 시 호출될 콜백</param>
    /// <param name="inventorySystem">이 컨트롤러가 관리하는 인벤토리 시스템</param>
    /// <param name="onItemInfoRequested">아이템 정보 표시 요청 콜백 (Dynamic 인벤토리에서만 사용)</param>
    public InventoryItemController(Dictionary<int, InventorySlot> slotMapping,
        IMouseItemView mouseItemView,
        System.Action<int> onSlotUpdated,
        InventorySystem inventorySystem,
        System.Action<ItemDataSO, int> onItemInfoRequested = null)
    {
        m_slotMapping = slotMapping;
        m_mouseItemView = mouseItemView;
        m_onSlotUpdated = onSlotUpdated;
        m_inventorySystem = inventorySystem;
        m_onItemInfoRequested = onItemInfoRequested;
    }

    /// <summary>
    /// 슬롯에서 아이템을 픽업
    /// Shift 키가 눌려있으면 절반의 수량만 픽업
    /// </summary>
    /// <param name="slot">픽업할 슬롯</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="partialKeyPressed">Shift 키 눌림 여부</param>
    public void PickupItem(InventorySlot slot, int slotIndex, bool partialKeyPressed)
    {
        if (partialKeyPressed && slot.SplitItemAmount(out var halfAmountSlot))
            m_mouseItemView.ShowItem(halfAmountSlot);
        else
        {
            var itemToPickup = new InventorySlot(slot.ItemDataSO, slot.ItemCount);
            slot.ClearSlot();
            m_mouseItemView.ShowItem(itemToPickup);
        }
        m_onSlotUpdated?.Invoke(slotIndex);
    }

    /// <summary>
    /// 슬롯 클릭 또는 드롭 시의 동작을 처리
    /// 마우스와 슬롯의 아이템 상태에 따라 픽업, 배치, 교환, 스택 등의 적절한 액션을 수행
    /// 인벤토리 간 아이템 이동 시에도 사용됨
    /// </summary>
    /// <param name="slotIndex">클릭한 슬롯 인덱스</param>
    /// <param name="clickedSlot">클릭한 슬롯 또는 마우스가 들고 있는 아이템</param>
    public void HandleSlotClick(int slotIndex, InventorySlot clickedSlot)
    {
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        var mouseItem = m_mouseItemView.GetCurrentItem();
        bool mouseEmpty = !m_mouseItemView.HasItem();

        //# Dynamic 인벤토리에서 아이템이 있는 슬롯을 클릭했을 때 아이템 정보 표시
        if (IsDynamicInventory() && clickedSlot.ItemDataSO != null && mouseEmpty)
            RequestItemInfo(clickedSlot.ItemDataSO, clickedSlot.ItemCount);
        //# 슬롯에 아이템이 없고, 마우스에 있는 경우
        else if (clickedSlot.ItemDataSO == null && !mouseEmpty) PlaceItem(clickedSlot, slotIndex, mouseItem);
        //# 슬롯에 아이템이 있고, 마우스에 있는 경우
        else if (clickedSlot.ItemDataSO != null && !mouseEmpty) HandleSlotInteraction(slotIndex, mouseItem);
    }

    /// <summary>
    /// 현재 인벤토리가 Dynamic 인벤토리인지 확인
    /// Inventory 컴포넌트의 DynamicInventorySystem과 비교
    /// </summary>
    /// <returns>Dynamic 인벤토리면 true, 아니면 false</returns>
    private bool IsDynamicInventory()
    {
        //# Inventory 컴포넌트를 찾아서 DynamicInventorySystem과 비교
        var inventory = Object.FindObjectOfType<Inventory>();
        return inventory != null && inventory.DynamicInventorySystem == m_inventorySystem;
    }

    /// <summary>
    /// 아이템 정보 표시를 요청
    /// Dynamic 인벤토리에서만 호출됨
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    private void RequestItemInfo(ItemDataSO itemData, int amount)
    {
        m_onItemInfoRequested?.Invoke(itemData, amount);
    }

    /// <summary>
    /// 마우스와 슬롯 모두에 아이템이 있을 때의 상호작용을 처리
    /// 아이템 타입에 따라 교환, 스택, 배치 중 적절한 동작을 수행
    /// </summary>
    /// <param name="slotIndex">대상 슬롯 인덱스</param>
    /// <param name="mouseItem">마우스가 들고 있는 아이템</param>
    private void HandleSlotInteraction(int slotIndex, InventorySlot mouseItem)
    {
        var slot = m_slotMapping[slotIndex];

        if (slot.ItemDataSO == null) PlaceItem(slot, slotIndex, mouseItem);
        else if (slot.ItemDataSO != mouseItem.ItemDataSO) SwapItems(slot, slotIndex, mouseItem);
        else StackItems(slot, slotIndex, mouseItem);
    }

    /// <summary>
    /// 마우스의 아이템을 빈 슬롯에 배치
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="mouseItem">배치할 아이템</param>
    private void PlaceItem(InventorySlot slot, int slotIndex, InventorySlot mouseItem)
    {
        slot.AddItem(mouseItem);
        m_mouseItemView.ClearItem();
        m_onSlotUpdated?.Invoke(slotIndex);
    }

    /// <summary>
    /// 마우스와 슬롯의 아이템을 서로 교환
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="mouseItem">마우스가 들고 있는 아이템</param>
    private void SwapItems(InventorySlot slot, int slotIndex, InventorySlot mouseItem)
    {
        var tempSlot = new InventorySlot(slot.ItemDataSO, slot.ItemCount);
        slot.ClearSlot();
        slot.AddItem(mouseItem);

        m_mouseItemView.ClearItem();
        m_mouseItemView.ShowItem(tempSlot);
        m_onSlotUpdated?.Invoke(slotIndex);
    }

    /// <summary>
    /// 같은 타입의 아이템을 스택
    /// 스택 한계를 초과하는 경우 부분 스택을 처리
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="mouseItem">스택할 아이템</param>
    private void StackItems(InventorySlot slot, int slotIndex, InventorySlot mouseItem)
    {
        if (slot.CanAdd(mouseItem.ItemCount))
        {
            slot.AddItem(mouseItem);
            m_mouseItemView.ClearItem();
        }
        else
            HandlePartialStack(slot, slotIndex, mouseItem);

        m_onSlotUpdated?.Invoke(slotIndex);
    }

    /// <summary>
    /// 부분 스택을 처리
    /// 슬롯에 들어갈 수 있는 만큼만 스택하고 나머지는 마우스 슬롯으로 분할
    /// </summary>
    /// <param name="slot">대상 슬롯</param>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="mouseItem">스택할 아이템</param>
    private void HandlePartialStack(InventorySlot slot, int slotIndex, InventorySlot mouseItem)
    {
        slot.CanAdd(mouseItem.ItemCount, out int canAddAmount);

        if (canAddAmount < 1)
        {
            SwapItems(slot, slotIndex, mouseItem);
            return;
        }

        int remainingOnMouse = mouseItem.ItemCount - canAddAmount;
        slot.AddItem(canAddAmount);

        var newMouseItem = new InventorySlot(mouseItem.ItemDataSO, remainingOnMouse);
        m_mouseItemView.ClearItem();
        m_mouseItemView.ShowItem(newMouseItem);
    }

    /// <summary>
    /// 지정된 슬롯에 아이템을 복원
    /// 주로 드래그 앤 드롭 실패 시 원래 위치로 되돌릴 때 사용
    /// </summary>
    /// <param name="slotIndex">복원할 슬롯 인덱스</param>
    /// <param name="itemToRestore">복원할 아이템</param>
    public void RestoreItem(int slotIndex, InventorySlot itemToRestore)
    {
        var slot = m_slotMapping[slotIndex];
        slot.ClearSlot();
        slot.AddItem(itemToRestore);

        m_onSlotUpdated?.Invoke(slotIndex);
    }
}