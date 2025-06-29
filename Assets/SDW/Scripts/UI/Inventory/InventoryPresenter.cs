using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 시스템의 MVP 패턴에서 Presenter 역할을 하는 클래스
/// 인벤토리 시스템(Model)과 UI(View) 사이의 중재자 역할과
/// 사용자 입력을 처리하고 UI 업데이트를 관리
/// </summary>
public class InventoryPresenter : MonoBehaviour
{
    private static readonly HashSet<IInventoryView> AllInventoryViews = new HashSet<IInventoryView>();

    private InventorySystem m_inventorySystem;
    public InventorySystem InventorySystem => m_inventorySystem;

    private IInventoryView m_inventoryView;
    private Dictionary<int, InventorySlot> m_slotMapping;

    private InventoryDragHandler m_dragHandler;
    private InventoryItemController m_itemController;
    private InventoryInputHandler m_inputHandler;

    private static Dictionary<InventorySystem, InventoryItemController> m_inventoryControllers =
        new Dictionary<InventorySystem, InventoryItemController>();

    /// <summary>
    /// Presenter를 초기화
    /// </summary>
    /// <param name="inventorySystem">인벤토리 시스템</param>
    /// <param name="inventoryView">인벤토리 뷰 인터페이스</param>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    /// <returns>초기화된 InventoryPresenter 인스턴스</returns>
    public InventoryPresenter Initialize(
        InventorySystem inventorySystem,
        IInventoryView inventoryView,
        IMouseItemView mouseItemView
    ) => Initialize(inventorySystem, inventoryView, mouseItemView, null);

    /// <summary>
    /// Presenter를 초기화
    /// 내부 초기화를 수행
    /// 이벤트 연결 및 슬롯 매핑을 설정
    /// </summary>
    /// <param name="inventorySystem">인벤토리 시스템</param>
    /// <param name="inventoryView">인벤토리 뷰 인터페이스</param>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    /// <param name="onItemInfoRequested">아이템 정보 표시 요청 콜백 (Dynamic 인벤토리에서만 사용)</param>
    /// <returns>초기화된 InventoryPresenter 인스턴스</returns>
    public InventoryPresenter Initialize(
        InventorySystem inventorySystem,
        IInventoryView inventoryView,
        IMouseItemView mouseItemView,
        Action<ItemDataSO, int> onItemInfoRequested
    )
    {
        m_inventorySystem = inventorySystem;
        m_inventoryView = inventoryView;
        m_slotMapping = new Dictionary<int, InventorySlot>();

        // InventoryItemController 생성 시 아이템 정보 표시 콜백을 전달
        m_itemController = new InventoryItemController(
            m_slotMapping,
            mouseItemView,
            UpdateSlotView,
            inventorySystem,
            onItemInfoRequested);

        m_dragHandler = new InventoryDragHandler(mouseItemView, m_itemController);
        m_inputHandler = new InventoryInputHandler(mouseItemView);

        m_inventoryControllers[m_inventorySystem] = m_itemController;

        RegisterInventoryView(m_inventoryView);

        m_inventorySystem.OnSlotChanged += OnSlotChanged;

        //# Dynamic Inventory의 첫 번째 슬롯은 Trash
        for (int i = 0; i < m_inventorySystem.InventorySize; i++)
        {
            m_slotMapping[i] = m_inventorySystem.InventorySlots[i];
            UpdateSlotView(i);
        }
        return this;
    }

    /// <summary>
    /// Dynamic Inventory의 경우, Enable 될 때 등록
    /// </summary>
    private void OnEnable() => RegisterInventoryView(m_inventoryView);

    /// <summary>
    /// Dynamic Inventory의 경우, Enable 될 때 등록해제
    /// </summary>
    private void OnDisable() => UnregisterInventoryView(m_inventoryView);

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
        if (slotIndex >= m_slotMapping.Count) return;

        var clickedSlot = m_slotMapping[slotIndex];
        m_itemController.HandleSlotClick(slotIndex, clickedSlot);
    }

    /// <summary>
    /// 드래그가 시작되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="slotIndex">드래그가 시작된 슬롯의 인덱스</param>
    public void OnBeginDrag(int slotIndex)
    {
        if (slotIndex >= m_slotMapping.Count) return;

        var slot = m_slotMapping[slotIndex];
        m_dragHandler.OnBeginDrag(slotIndex, slot);
    }

    /// <summary>
    /// 드래그가 종료되었을 때 호출되는 메서드
    /// </summary>
    /// <param name="targetSlotIndex">드롭 대상 슬롯 인덱스</param>
    /// <param name="isValidDrop">유효한 드롭인지 여부</param>
    /// <param name="isOutsideInventory">인벤토리 영역 밖으로 드롭했는지 여부</param>
    /// <param name="targetInventorySystem">대상 인벤토리 시스템. null일 경우 자신의 인벤토리 시스템 사용</param>
    public void OnEndDrag(
        int targetSlotIndex,
        bool isValidDrop,
        bool isOutsideInventory,
        InventorySystem targetInventorySystem = null,
        bool isTrashDrop = false
    )
    {
        InventoryItemController targetController = null;

        if (!targetInventorySystem.Equals(null) && targetInventorySystem != m_inventorySystem)
            m_inventoryControllers.TryGetValue(targetInventorySystem, out targetController);

        m_dragHandler.OnEndDrag(targetSlotIndex, isValidDrop, isOutsideInventory, targetController, isTrashDrop);
    }

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
    /// 모든 슬롯의 UI를 업데이트
    /// </summary>
    public void UpdateSlots()
    {
        foreach (var mapping in m_slotMapping)
        {
            UpdateSlotView(mapping.Key);
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
        if (!m_inventorySystem.Equals(null)) m_inventorySystem.OnSlotChanged -= OnSlotChanged;

        m_inventorySystem = null;
        m_inventoryView = null;
        m_slotMapping?.Clear();
        m_slotMapping = null;
    }

    /// <summary>
    /// 인벤토리 뷰를 활성 리스트에 등록
    /// InventoryView가 활성화될 때 호출되어야 함
    /// </summary>
    /// <param name="inventoryView">등록할 인벤토리 뷰(Interface)</param>
    private static void RegisterInventoryView(IInventoryView inventoryView)
    {
        if (inventoryView != null)
            AllInventoryViews.Add(inventoryView);
    }

    /// <summary>
    /// 인벤토리 뷰를 활성 리스트에서 제거
    /// InventoryView가 비활성화될 때 호출되어야 함
    /// </summary>
    /// <param name="inventoryView">제거할 인벤토리 뷰(Interface)</param>
    private static void UnregisterInventoryView(IInventoryView inventoryView)
    {
        if (inventoryView != null)
            AllInventoryViews.Remove(inventoryView);
    }

    /// <summary>
    /// SlotView에서 View 목록에 접근하기 위한 메서드
    /// </summary>
    /// <returns>InventroyView Hash Set을 반환</returns>
    public HashSet<IInventoryView> GetAllViews() => AllInventoryViews;
}