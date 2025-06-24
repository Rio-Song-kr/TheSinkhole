using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 인벤토리 컨테이너 역할
/// Player에 추가되며 Inventory 크기만큼 InventorySystem을 통해 슬롯을 생성하며 각 슬롯을 관리
/// Inventory와 관련된 모델(데이터 계층) 역할
/// </summary>
[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int m_inventorySize;

    [SerializeField] private InventorySystem m_inventorySystem;
    public InventorySystem InventorySystem => m_inventorySystem;

    private void Awake() => m_inventorySystem = new InventorySystem(m_inventorySize);
}