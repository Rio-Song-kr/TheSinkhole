using UnityEngine;

/// <summary>
/// 백팩, 상자 등을 위한 인벤토리 UI를 담당하는 클래스
/// InventoryView를 상속받아 Dynamic 전용 인벤토리 뷰를 구현
/// 지정된 Inventory 오브젝트와 연결되어 백팩 기능을 제공
/// </summary>
public class DynamicInventoryView : InventoryView
{
    private InventorySystem m_dynamicInventorySystem;

    /// <summary>
    /// 백팩 뷰를 초기화
    /// 연결된 InventorySystem이 있다면 해당 인벤토리 시스템으로 초기화를 진행
    /// </summary>
    public void RefreshDisplaySlots(InventorySystem inventorySystem)
    {
        if (inventorySystem != null) Initialize(inventorySystem);
        else Debug.LogWarning($"{gameObject.name}: 백팩이 정의되지 않았습니다.");
    }
}