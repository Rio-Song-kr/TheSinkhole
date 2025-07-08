using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 개별 인벤토리 슬롯의 UI 표시 및 상호작용을 담당하는 클래스
/// 아이템 아이콘, 개수 표시 및 클릭, 드래그 앤 드롭 기능을 제공
/// Unity EventSystem과 연동하여 포인터 이벤트를 처리하고 인벤토리 간 아이템 이동을 지원
/// </summary>
public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private SlotType m_slotType = SlotType.Normal;
    public bool IsTrashSlot => m_slotType == SlotType.Trash;

    private Image m_itemSprite;
    private TextMeshProUGUI m_itemCount;

    private int m_slotIndex;
    private InventoryPresenter m_presenter;

    private InventorySystem m_inventorySystem;

    /// <summary>
    /// 컴포넌트 참조를 초기화
    /// </summary>
    private void Awake()
    {
        var itemSprites = GetComponentsInChildren<Image>();
        m_itemSprite = itemSprites.Length > 1 ? itemSprites[1] : itemSprites[0];

        if (m_slotType == SlotType.Normal && m_itemCount == null)
            m_itemCount = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 뷰를 초기화
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    /// <param name="presenter">연결할 Presenter</param>
    public void Initialize(int slotIndex, InventoryPresenter presenter)
    {
        m_slotIndex = slotIndex;
        m_presenter = presenter;

        if (m_itemSprite == null)
        {
            m_itemSprite = GetComponentInChildren<Image>();
        }

        if (m_slotType == SlotType.Normal && m_itemCount == null)
        {
            m_itemCount = GetComponentInChildren<TextMeshProUGUI>();
        }

        m_inventorySystem = presenter.InventorySystem;

        ClearDisplay();
    }

    /// <summary>
    /// 슬롯의 아이템 표시를 업데이트
    /// </summary>
    /// <param name="slot">표시할 슬롯 데이터</param>
    public void UpdateDisplay(InventorySlot slot)
    {
        if (slot.ItemDataSO != null)
        {
            if (m_slotType != SlotType.Trash)
                m_itemSprite.sprite = slot.ItemDataSO.Icon;
            m_itemSprite.color = Color.white;
            if (m_slotType == SlotType.Normal)
                m_itemCount.text = slot.ItemCount > 1 ? slot.ItemCount.ToString() : "";
        }
        else
        {
            ClearDisplay();
        }
    }

    /// <summary>
    /// 슬롯 표시를 제거
    /// </summary>
    public void ClearDisplay()
    {
        if (m_slotType != SlotType.Trash)
        {
            m_itemSprite.sprite = null;
            m_itemSprite.color = Color.clear;
        }
        if (m_slotType == SlotType.Normal)
            m_itemCount.text = "";
    }

    /// <summary>
    /// 포인터 클릭 이벤트 핸들러
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        m_presenter?.OnSlotClicked(m_slotIndex);
    }

    /// <summary>
    /// 드래그 시작 이벤트 핸들러
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnBeginDrag(PointerEventData eventData) => m_presenter?.OnBeginDrag(m_slotIndex);

    /// <summary>
    /// 드래그 중 이벤트 핸들러
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnDrag(PointerEventData eventData)
    {
        //! 드래그 중에는 마우스 아이템 뷰가 위치를 업데이트
        //# 추후 추가적인 기능 구현이 필요할 가능성이 있어 미리 메서드만 추가
    }

    /// <summary>
    /// 드래그 종료 이벤트 핸들러
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        var targetSlot = GetDropTarget(eventData, out var targetSystem);

        bool isValidDrop = targetSlot != null;
        bool isOutsideInventory = targetSlot != null ? false : !IsPointerOverInventory(eventData);

        bool isTrashDrop = targetSlot != null && targetSlot.IsTrashSlot;

        int targetIndex = isValidDrop ? targetSlot.m_slotIndex : -1;
        m_presenter?.OnEndDrag(targetIndex, isValidDrop, isOutsideInventory, targetSystem, isTrashDrop);
    }

    /// <summary>
    /// 드롭 대상 슬롯을 찾고 해당 슬롯이 속한 인벤토리 시스템을 반환
    /// 인벤토리 간 아이템 이동을 위해 타겟 시스템 정보도 함께 제공
    /// </summary>
    /// <param name="eventData">포인터 이벤트 데이터</param>
    /// <param name="targetSystem">드롭 대상 슬롯이 속한 인벤토리 시스템</param>
    /// <returns>드롭 대상 슬롯 뷰 (없으면 null)</returns>
    private InventorySlotView GetDropTarget(PointerEventData eventData, out InventorySystem targetSystem)
    {
        targetSystem = m_inventorySystem;

        foreach (var go in eventData.hovered)
        {
            var slotView = go.GetComponent<InventorySlotView>();

            // if (slotView != null && slotView != this) return slotView;
            if (slotView != null)
            {
                targetSystem = slotView.m_inventorySystem;
                return slotView;
            }
        }

        return null;
    }

    /// <summary>
    /// 포인터가 인벤토리 영역 내에 있는지 확인
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    /// <returns>인벤토리 영역 내에 있으면 true</returns>
    private bool IsPointerOverInventory(PointerEventData eventData) => IsInsideInventoryArea(eventData.position);

    /// <summary>
    /// 지정된 위치가 현재 슬롯이 속한 인벤토리 영역 내부에 있는지 확인
    /// RectTransform을 사용하여 화면 좌표를 기준으로 영역 검사 수행
    /// </summary>
    /// <param name="position">확인할 화면 위치</param>
    /// <returns>인벤토리 영역 내부에 있으면 true, 아니면 false</returns>
    private bool IsInsideInventoryArea(Vector3 position)
    {
        var iInventoryViews = m_presenter.GetAllViews();

        foreach (var iInventoryView in iInventoryViews)
        {
            var inventoryView = iInventoryView as InventoryView;
            if (!inventoryView.gameObject.activeInHierarchy) continue;

            var rectTransform = inventoryView.GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, position)) return true;
        }

        return false;
    }
}