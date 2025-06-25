using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 백팩, 상자 등을 위한 인벤토리 UI를 담당하는 클래스
/// InventoryView를 상속받아 Dynamic 전용 인벤토리 뷰를 구현
/// 지정된 Inventory 오브젝트와 연결되어 백팩 기능을 제공
/// </summary>
public class DynamicInventoryView : InventoryView
{
    [Header("아이템 정보 UI")]
    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_trashButton;
    [SerializeField] private Image m_itemIcon;
    [SerializeField] private TextMeshProUGUI m_itemNameText;
    [SerializeField] private TextMeshProUGUI m_itemAmountText;
    [SerializeField] private TextMeshProUGUI m_itemDescriptionText;

    private InventorySystem m_dynamicInventorySystem;

    /// <summary>
    /// 백팩 뷰를 초기화
    /// 연결된 InventorySystem이 있다면 해당 인벤토리 시스템으로 초기화를 진행
    /// </summary>
    public void RefreshDisplaySlots(InventorySystem inventorySystem)
    {
        if (inventorySystem != null)
        {
            m_dynamicInventorySystem = inventorySystem;
            Initialize(m_dynamicInventorySystem);
        }
        else
            Debug.LogWarning($"{gameObject.name}: 백팩이 정의되지 않았습니다.");
    }

    /// <summary>
    /// 인벤토리 뷰를 초기화하고 프레젠터와 연결
    /// Dynamic 인벤토리에서는 아이템 정보 표시 기능을 추가로 설정
    /// </summary>
    /// <param name="inventorySystem">연결할 인벤토리 시스템</param>
    public override void Initialize(InventorySystem inventorySystem)
    {
        //# MouseItemView를 자동으로 찾아서 Presenter에 전달
        var mouseItemView = FindObjectOfType<MouseItemView>();
        if (mouseItemView == null)
        {
            Debug.LogError($"{gameObject.name}: MouseItemView를 찾을 수 없습니다.");
            return;
        }

        //# Presenter 생성 - Dynamic 인벤토리에서는 아이템 정보 표시 콜백을 전달
        m_presenter = m_presenter.Initialize(inventorySystem, this, mouseItemView, OnItemInfoRequested);

        //# 각 슬롯 뷰를 Presenter와 연결
        for (int i = 0; i < m_slotViews.Length; i++)
        {
            m_slotViews[i].Initialize(i, m_presenter);
            m_presenter.UpdateSlots();
        }

        ClearItemInfo();
    }

    /// <summary>
    /// 아이템 정보 표시 요청을 처리하는 콜백
    /// Dynamic 인벤토리에서 아이템이 클릭되었을 때 호출됨
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    /// <param name="amount">표시할 아이템의 개수</param>
    private void OnItemInfoRequested(ItemDataSO itemData, int amount)
    {
        if (itemData == null)
        {
            ClearItemInfo();
            return;
        }

        DisplayItemInfo(itemData, amount);
    }

    /// <summary>
    /// 아이템 정보를 UI에 표시
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    /// <param name="amount">표시할 아이템의 개수</param>
    private void DisplayItemInfo(ItemDataSO itemData, int amount)
    {
        if (m_itemNameText != null) m_itemNameText.text = itemData.ItemData.ItemName;

        if (m_itemDescriptionText != null) m_itemDescriptionText.text = itemData.ItemText;

        if (m_itemAmountText != null) m_itemAmountText.text = $"Amount : {amount.ToString()}";

        if (m_itemIcon != null) m_itemIcon.sprite = itemData.Icon;
    }

    /// <summary>
    /// 아이템 정보 UI를 숨김
    /// </summary>
    private void ClearItemInfo()
    {
        if (m_itemNameText != null) m_itemNameText.text = "";

        if (m_itemDescriptionText != null) m_itemDescriptionText.text = "";

        if (m_itemAmountText != null) m_itemAmountText.text = "";

        if (m_itemIcon != null) m_itemIcon.sprite = null;
    }

    /// <summary>
    /// Dynamic 인벤토리가 비활성화될 때 아이템 정보도 숨김
    /// </summary>
    private void OnDisable()
    {
        ClearItemInfo();
    }
}