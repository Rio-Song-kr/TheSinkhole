using UnityEngine;

/// <summary>
/// 인벤토리 UI의 기본 뷰 클래스
/// MVP 패턴의 View 역할을 담당하며, 인벤토리 슬롯들의 시각적 표현을 관리
/// 상속받는 클래스에서 구체적인 인벤토리 타입(플레이어, 상점 등)을 구현
/// </summary>
public abstract class InventoryView : MonoBehaviour, IInventoryView
{
    protected InventoryPresenter m_presenter;
    protected InventorySlotView[] m_slotViews;

    /// <summary>
    /// 컴포넌트 초기화
    /// 자식 객체에서 InventorySlotView 컴포넌트들을 찾아서 배열로 저장
    /// </summary>
    protected virtual void Awake()
    {
        m_slotViews = GetComponentsInChildren<InventorySlotView>();
        m_presenter = GetComponent<InventoryPresenter>();
    }

    /// <summary>
    /// 게임 오브젝트 시작 시 호출
    /// 상속받는 클래스에서 오버라이드하여 초기화 로직 구현
    /// </summary>
    protected virtual void Start()
    {
    }

    /// <summary>
    /// 인벤토리 뷰를 초기화하고 프레젠터와 연결
    /// MouseItemView를 자동으로 찾아서 드래그 앤 드롭 기능을 설정
    /// </summary>
    /// <param name="inventorySystem">연결할 인벤토리 시스템</param>
    public virtual void Initialize(InventorySystem inventorySystem)
    {
        //# MouseItemView를 자동으로 찾아서 Presenter에 전달
        var mouseItemView = FindObjectOfType<MouseItemView>();
        if (mouseItemView == null)
        {
            Debug.LogError($"{gameObject.name}: MouseItemView를 찾을 수 없습니다.");
            return;
        }

        //# Presenter 생성 - 두 개의 View를 독립적으로 관리
        m_presenter = m_presenter.Initialize(inventorySystem, this, mouseItemView);

        //# 각 슬롯 뷰를 Presenter와 연결
        for (int i = 0; i < m_slotViews.Length; i++)
        {
            m_slotViews[i].Initialize(i, m_presenter);
            m_presenter.UpdateSlots();
        }
    }

    /// <summary>
    /// 지정된 슬롯의 UI 표시를 업데이트
    /// 아이템 아이콘, 개수 등의 시각적 요소를 갱신
    /// </summary>
    /// <param name="slotIndex">업데이트할 슬롯의 인덱스</param>
    /// <param name="slot">표시할 슬롯 데이터</param>
    public void UpdateSlotDisplay(int slotIndex, InventorySlot slot)
    {
        if (slotIndex >= 0 && slotIndex < m_slotViews.Length && m_slotViews[slotIndex] != null)
            m_slotViews[slotIndex].UpdateDisplay(slot);
        else
            Debug.LogWarning($"{gameObject.name}: 잘못된 슬롯 인덱스 또는 슬롯 뷰가 null입니다. Index: {slotIndex}");
    }

    /// <summary>
    /// 지정된 슬롯의 UI 표시를 비움
    /// 빈 슬롯 상태로 시각적 요소를 초기화
    /// </summary>
    /// <param name="slotIndex">비울 슬롯의 인덱스</param>
    public void ClearSlotDisplay(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < m_slotViews.Length)
            m_slotViews[slotIndex].ClearDisplay();
        else
            Debug.LogWarning($"{gameObject.name}: 잘못된 슬롯 인덱스 또는 슬롯 뷰가 null입니다. Index: {slotIndex}");
    }
}