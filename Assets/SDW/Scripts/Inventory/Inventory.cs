using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Player에 추가되며 Inventory 크기만큼 InventorySystem을 통해 슬롯을 생성하며 각 슬롯을 관리
/// </summary>
[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int m_inventorySize;

    [SerializeField] private InventorySystem m_inventorySystem;
    public InventorySystem MInventorySystem => m_inventorySystem;

    public static UnityAction<InventorySystem> OnUpdateInventoryUI;

    private void Awake() => m_inventorySystem = new InventorySystem(m_inventorySize);
}