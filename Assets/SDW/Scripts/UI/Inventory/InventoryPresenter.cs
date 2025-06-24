using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 시스템의 MVP 패턴에서 Presenter 역할을 하는 클래스
/// 인벤토리 시스템(Model)과 UI(View) 사이의 중재자 역할과
/// 사용자 입력을 처리하고 UI 업데이트를 관리
/// </summary>
public class InventoryPresenter : MonoBehaviour
{
    private InventorySystem m_inventorySystem;
    private IInventoryView m_inventoryView;
    private Dictionary<int, InventorySlot> m_slotMapping;

    // 분리된 핸들러들
    private InventoryDragHandler m_dragHandler;
    private InventoryItemController m_itemController;
    private InventoryInputHandler m_inputHandler;

    /// <summary>
    /// Presenter를 초기화
    /// </summary>
    /// <param name="inventorySystem">인벤토리 시스템</param>
    /// <param name="inventoryView">인벤토리 뷰 인터페이스</param>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    /// <returns>초기화된 InventoryPresenter 인스턴스</returns>
    public InventoryPresenter Initialize(InventorySystem inventorySystem,
        IInventoryView inventoryView,
        IMouseItemView mouseItemView)
    {
        m_inventorySystem = inventorySystem;
        m_inventoryView = inventoryView;
        m_slotMapping = new Dictionary<int, InventorySlot>();

        //# 핸들러들 초기화
        m_itemController = new InventoryItemController(m_slotMapping, mouseItemView, UpdateSlotView);
        m_dragHandler = new InventoryDragHandler(mouseItemView, m_itemController);
        m_inputHandler = new InventoryInputHandler(mouseItemView);

        Initialize();
        return this;
    }

    /// <summary>
    /// 내부 초기화를 수행
    /// 이벤트 연결 및 슬롯 매핑을 설정
    /// </summary>
    private void Initialize()
    {
        m_inventorySystem.OnSlotChanged += OnSlotChanged;

        for (int i = 0; i < m_inventorySystem.InventorySize; i++)
        {
            m_slotMapping[i] = m_inventorySystem.InventorySlots[i];
            UpdateSlotView(i);
        }
    }

    /// <summary>
    /// 프레임마다 호출되어 입력을 처리
    /// </summary>
    private void Update() => m_inputHandler.HandleMouseInput();

    /// <summary>
    /// 슬롯이 클릭되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex">클릭된 슬롯의 인덱스</param>
    public void OnSlotClicked(int slotIndex)
    {
        var clickedSlot = m_slotMapping[slotIndex];

        m_itemController.HandleSlotClick(slotIndex, clickedSlot);
    }

    /// <summary>
    /// 드래그가 시작되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex">드래그가 시작된 슬롯의 인덱스</param>
    public void OnBeginDrag(int slotIndex)
    {
        var slot = m_slotMapping[slotIndex];
        m_dragHandler.OnBeginDrag(slotIndex, slot);
    }

    /// <summary>
    /// 드래그가 종료되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="targetSlotIndex">드롭 대상 슬롯 인덱스</param>
    /// <param name="isValidDrop">유효한 드롭인지 여부</param>
    /// <param name="isOutsideInventory">인벤토리 영역 밖으로 드롭했는지 여부</param>
    public void OnEndDrag(int targetSlotIndex, bool isValidDrop, bool isOutsideInventory)
        => m_dragHandler.OnEndDrag(targetSlotIndex, isValidDrop, isOutsideInventory);

    /// <summary>
    /// 인벤토리 시스템에서 슬롯이 변경되었을 때 호출되는 이벤트 핸들러
    /// </summary>
    /// <param name="slot">변경된 슬롯</param>
    private void OnSlotChanged(InventorySlot slot)
    {
        foreach (var mapping in m_slotMapping)
        {
            if (mapping.Value == slot)
            {
                UpdateSlotView(mapping.Key);
                break;
            }
        }
    }

    /// <summary>
    /// 지정된 슬롯의 UI를 업데이트
    /// </summary>
    /// <param name="slotIndex">업데이트할 슬롯 인덱스</param>
    private void UpdateSlotView(int slotIndex)
    {
        var slot = m_slotMapping[slotIndex];

        if (slot.ItemDataSO != null) m_inventoryView.UpdateSlotDisplay(slotIndex, slot);
        else m_inventoryView.ClearSlotDisplay(slotIndex);
    }

    /// <summary>
    /// Presenter를 정리하고 리소스를 해제
    /// </summary>
    public void Cleanup()
    {
        if (m_inventorySystem != null) m_inventorySystem.OnSlotChanged -= OnSlotChanged;

        m_inventorySystem = null;
        m_inventoryView = null;
        m_slotMapping?.Clear();
        m_slotMapping = null;
    }
}