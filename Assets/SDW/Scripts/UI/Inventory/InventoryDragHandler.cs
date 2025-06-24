using UnityEngine;

/// <summary>
/// 인벤토리 아이템의 드래그 앤 드롭 기능을 처리하는 핸들러 클래스
/// </summary>
public class InventoryDragHandler
{
    private int m_originalSlotIndex = -1;
    private InventorySlot m_slotBackup;
    private IMouseItemView m_mouseItemView;
    private InventoryItemController m_itemController;

    /// <summary>
    /// InventoryDragHandler 생성자
    /// </summary>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    /// <param name="itemController">아이템 컨트롤러</param>
    public InventoryDragHandler(IMouseItemView mouseItemView, InventoryItemController itemController)
    {
        m_mouseItemView = mouseItemView;
        m_itemController = itemController;
        m_slotBackup = new InventorySlot();
    }

    /// <summary>
    /// 드래그 시작 시 호출되는 메서드
    /// 원본 슬롯 상태를 저장하고 아이템을 픽업
    /// </summary>
    /// <param name="slotIndex">드래그 시작 슬롯 인덱스</param>
    /// <param name="slot">드래그 시작 슬롯</param>
    public void OnBeginDrag(int slotIndex, InventorySlot slot)
    {
        if (slot.ItemDataSO != null && !m_mouseItemView.HasItem())
        {
            SaveOriginalSlotState(slotIndex, slot);
            bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
            m_itemController.PickupItem(slot, slotIndex, shiftPressed);
        }
    }

    /// <summary>
    /// 드래그 종료 시 호출되는 메서드
    /// 드롭 유효성에 따라 아이템 배치 또는 복원을 처리
    /// </summary>
    /// <param name="targetSlotIndex">드롭 대상 슬롯 인덱스</param>
    /// <param name="isValidDrop">유효한 드롭인지 여부</param>
    /// <param name="isOutsideInventory">인벤토리 영역 밖으로 드롭했는지 여부</param>
    public void OnEndDrag(int targetSlotIndex, bool isValidDrop, bool isOutsideInventory)
    {
        //# 마우스가 인벤토리 영역 밖에 있는 경우
        if (isOutsideInventory)
        {
            m_mouseItemView.ClearItem();
            ClearOriginalSlotState();
            return;
        }

        //# 마우스가 인벤토리 영역 안에 있지만, 슬롯 영역이 아닌 경우
        if (!isValidDrop)
        {
            RestoreToOriginalSlot();
            return;
        }

        //# 마우스가 인벤토리 영역 안에 있고, 슬롯 영역에 영역에 있으면서 아이템을 가지고 있는 경우
        if (m_mouseItemView.HasItem())
        {
            var mouseItem = m_mouseItemView.GetCurrentItem();
            m_itemController.HandleSlotClick(targetSlotIndex, mouseItem);
        }

        ClearOriginalSlotState();
    }

    /// <summary>
    /// 원본 슬롯의 상태를 백업으로 저장
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="slot">백업할 슬롯</param>
    private void SaveOriginalSlotState(int slotIndex, InventorySlot slot)
    {
        m_originalSlotIndex = slotIndex;
        m_slotBackup.ClearSlot();

        if (slot.ItemDataSO != null) m_slotBackup.AddItem(slot);
    }

    /// <summary>
    /// 저장된 원본 슬롯 상태를 초기화
    /// </summary>
    private void ClearOriginalSlotState()
    {
        m_originalSlotIndex = -1;
        m_slotBackup.ClearSlot();
    }

    /// <summary>
    /// 아이템을 원본 슬롯 위치로 복원
    /// </summary>
    private void RestoreToOriginalSlot()
    {
        if (m_originalSlotIndex >= 0 && m_slotBackup.ItemDataSO != null)
            m_itemController.RestoreItem(m_originalSlotIndex, m_slotBackup);

        m_mouseItemView.ClearItem();
        ClearOriginalSlotState();
    }
}