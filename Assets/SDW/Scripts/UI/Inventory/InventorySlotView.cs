using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 개별 인벤토리 슬롯의 UI 표시 및 상호작용을 담당하는 클래스
/// 아이템 아이콘, 개수 표시 및 클릭, 드래그 앤 드롭 기능을 제공
/// </summary>
public class InventorySlotView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Image m_itemSprite;
    private TextMeshProUGUI m_itemCount;

    private int m_slotIndex;
    private InventoryPresenter m_presenter;

    /// <summary>
    /// 컴포넌트 참조를 초기화
    /// </summary>
    private void Awake()
    {
        var itemSprites = GetComponentsInChildren<Image>();
        m_itemSprite = itemSprites.Length > 1 ? itemSprites[1] : itemSprites[0];

        if (m_itemCount == null)
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

        if (m_itemCount == null)
        {
            m_itemCount = GetComponentInChildren<TextMeshProUGUI>();
        }

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
            m_itemSprite.sprite = slot.ItemDataSO.Icon;
            m_itemSprite.color = Color.white;
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
        m_itemSprite.sprite = null;
        m_itemSprite.color = Color.clear;
        m_itemCount.text = "";
    }

    /// <summary>
    /// 포인터 클릭 이벤트 핸들러
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        //todo 추후 Slot을 클릭할 땐, 아이템 정보가 있을 때 해당 정보를 표시하도록 수정해야 함
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
        var targetSlot = GetDropTarget(eventData);
        bool isValidDrop = targetSlot != null;
        bool isOutsideInventory = !IsPointerOverInventory(eventData);

        int targetIndex = isValidDrop ? targetSlot.m_slotIndex : -1;
        m_presenter?.OnEndDrag(targetIndex, isValidDrop, isOutsideInventory);
    }

    /// <summary>
    /// 드롭 대상 슬롯을 찾음
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    /// <returns>드롭 대상 슬롯 (없으면 null)</returns>
    private InventorySlotView GetDropTarget(PointerEventData eventData)
    {
        foreach (var go in eventData.hovered)
        {
            var slotView = go.GetComponent<InventorySlotView>();

            if (slotView != null && slotView != this) return slotView;
        }
        return null;
    }

    /// <summary>
    /// 포인터가 인벤토리 영역 내에 있는지 확인
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    /// <returns>인벤토리 영역 내에 있으면 true</returns>
    private bool IsPointerOverInventory(PointerEventData eventData) => IsInsideInventoryArea(eventData.position);

    private bool IsInsideInventoryArea(Vector3 position)
    {
        var inventoryView = GetComponentInParent<InventoryView>();
        if (inventoryView == null) return false;

        var canvasRect = inventoryView.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(canvasRect, position);
    }
}