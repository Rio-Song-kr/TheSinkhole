using UnityEngine;

/// <summary>
/// 퀵슬롯 UI를 담당하는 클래스
/// InventoryView를 상속받아 퀵슬롯 전용 인벤토리 뷰를 구현
/// 지정된 Inventory 오브젝트와 연결되어 퀵슬롯 기능을 제공
/// </summary>
public class QuickSlotView : InventoryView
{
    [SerializeField] private Inventory m_inventory;

    /// <summary>
    /// 퀵슬롯 뷰를 초기화
    /// 연결된 인벤토리가 있다면 해당 인벤토리 시스템으로 초기화를 진행
    /// </summary>
    protected override void Start()
    {
        base.Start();

        if (m_inventory != null)
        {
            Initialize(m_inventory.InventorySystem);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: 인벤토리가 정의되지 않았습니다.");
        }
    }
}