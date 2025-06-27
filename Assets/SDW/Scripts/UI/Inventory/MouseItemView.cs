using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 마우스 커서를 따라다니는 아이템 표시 UI 클래스
/// 드래그 앤 드롭 시 마우스 포인터에 아이템을 시각적으로 표시하고 실시간 위치 추적
/// UI 레이캐스트를 통한 인벤토리 영역 내외부 감지 및 드롭 유효성 검사 기능 제공
/// 전역적으로 하나의 인스턴스만 존재하며 모든 인벤토리가 공유하여 사용
/// </summary>
public class MouseItemView : MonoBehaviour, IMouseItemView
{
    [SerializeField] private Image m_itemSprite;
    [SerializeField] private TextMeshProUGUI m_itemCount;

    private InventorySlot m_currentItem;
    private GameObject m_player;

    /// <summary>
    /// 컴포넌트 초기화 및 기본 설정
    /// 필요한 UI 컴포넌트들을 자동으로 찾고 빈 아이템 상태로 초기화
    /// </summary>
    private void Awake()
    {
        if (m_itemSprite == null)
            m_itemSprite = GetComponentInChildren<Image>();

        if (m_itemCount == null)
            m_itemCount = GetComponentInChildren<TextMeshProUGUI>();

        m_currentItem = new InventorySlot();
        ClearItem();
    }

    /// <summary>
    /// 플레이어 오브젝트 참조를 초기화
    /// Player 태그를 가진 게임 오브젝트를 찾아 저장
    /// </summary>
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");

        if (m_player == null) Debug.LogWarning("Player 태그가 추가된 오브젝트가 없습니다.");
    }

    /// <summary>
    /// 매 프레임마다 아이템을 들고 있을 때 마우스 위치를 업데이트
    /// </summary>
    private void Update()
    {
        if (HasItem())
            UpdatePosition();
    }

    /// <summary>
    /// 마우스에 아이템을 표시하고 드래그 상태로 전환
    /// 아이템 아이콘과 개수를 화면에 표시하고 현재 아이템 정보를 저장
    /// </summary>
    /// <param name="slot">표시할 아이템 슬롯</param>
    public void ShowItem(InventorySlot slot)
    {
        m_currentItem.ClearSlot();
        m_currentItem.AddItem(slot);

        m_itemSprite.sprite = slot.ItemDataSO.Icon;
        m_itemSprite.color = Color.white;
        m_itemCount.text = slot.ItemCount > 1 ? slot.ItemCount.ToString() : "";
    }

    /// <summary>
    /// 마우스에서 아이템을 제거
    /// 모든 시각적 요소를 숨기고 현재 아이템 정보를 초기화
    /// </summary>
    public void ClearItem()
    {
        m_currentItem.ClearSlot();
        m_itemSprite.sprite = null;
        m_itemSprite.color = Color.clear;
        m_itemCount.text = "";
    }

    /// <summary>
    /// 현재 들고 있는 아이템을 월드에 드롭
    /// 플레이어 앞쪽 2미터 위치에 아이템을 생성하고 마우스 아이템을 초기화
    /// </summary>
    public void DropItem()
    {
        string itemName = m_currentItem.ItemDataSO.ItemData.ItemName;
        var item = GameManager.Instance.Item.ItemPools[itemName].Pool.Get();
        item.transform.position = m_player.transform.position + m_player.transform.forward * 2f;
        item.ItemAmount = m_currentItem.ItemCount;
        item.transform.rotation = Quaternion.identity;

        ClearItem();
    }

    /// <summary>
    /// 마우스 위치에 따라 UI 위치를 업데이트
    /// 화면 좌표계를 사용하여 마우스 커서를 따라다님
    /// </summary>
    public void UpdatePosition() => transform.position = Input.mousePosition;

    /// <summary>
    /// 현재 마우스에 들고 있는 아이템 슬롯을 반환
    /// </summary>
    /// <returns>현재 아이템 슬롯</returns>
    public InventorySlot GetCurrentItem() => m_currentItem;

    /// <summary>
    /// 마우스에 아이템을 들고 있는지 확인
    /// </summary>
    /// <returns>아이템을 들고 있으면 true, 아니면 false</returns>
    public bool HasItem() => m_currentItem.ItemDataSO != null;

    /// <summary>
    /// 지정된 화면 위치가 인벤토리 UI 영역 내부에 있는지 확인
    /// UI 레이캐스트를 사용하여 UI 요소 위에 있는지 검사
    /// </summary>
    /// <param name="screenPosition">확인할 화면 위치</param>
    /// <returns>인벤토리 영역 내부에 있으면 true, 아니면 false</returns>
    public bool IsInsideInventoryArea(Vector2 screenPosition) => IsPointerOverUIObject(screenPosition);

    /// <summary>
    /// 지정된 화면 위치에 UI 요소가 있는지 확인하는 내부 메서드
    /// EventSystem을 사용하여 UI 레이캐스트 수행
    /// </summary>
    /// <param name="screenPosition">확인할 화면 위치</param>
    /// <returns>UI 요소가 있으면 true, 아니면 false</returns>
    private bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}