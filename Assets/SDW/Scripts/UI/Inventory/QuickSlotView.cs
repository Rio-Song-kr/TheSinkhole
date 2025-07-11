using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀵슬롯 UI를 담당하는 클래스
/// InventoryView를 상속받아 퀵슬롯 전용 인벤토리 뷰를 구현
/// 지정된 Inventory 오브젝트와 연결되어 퀵슬롯 기능을 제공
/// </summary>
public class QuickSlotView : InventoryView
{
    /// <summary>
    /// 퀵슬롯과 연결될 인벤토리 컴포넌트
    /// Inspector에서 할당하며, 이 인벤토리의 QuickSlotInventorySystem을 사용
    /// </summary>
    [SerializeField] private Inventory m_quickSlotInventory;
    [SerializeField] private Color m_selectedColor;
    [SerializeField] private Color m_originalColor;

    private int m_prevIndex = 0;

    private void OnEnable() => Inventory.OnSelectedItemChanged += OnSelectedItemChanged;
    private void OnDisable() => Inventory.OnSelectedItemChanged += OnSelectedItemChanged;

    /// <summary>
    /// 퀵슬롯 뷰를 초기화
    /// 연결된 퀵슬롯이 있다면 해당 인벤토리 시스템으로 초기화를 진행
    /// </summary>
    protected override void Start()
    {
        base.Start();

        if (m_quickSlotInventory != null) Initialize(m_quickSlotInventory.QuickSlotInventorySystem);
        else Debug.LogWarning($"{gameObject.name}: 인벤토리가 정의되지 않았습니다.");
    }

    private void OnSelectedItemChanged(int index)
    {
        var prevSlotColor = m_slotViews[m_prevIndex].GetComponent<Image>();
        prevSlotColor.color = m_originalColor;

        var currentSlotColor = m_slotViews[index].GetComponent<Image>();
        currentSlotColor.color = m_selectedColor;

        m_prevIndex = index;
    }
}