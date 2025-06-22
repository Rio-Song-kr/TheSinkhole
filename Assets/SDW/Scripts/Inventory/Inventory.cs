using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Player에 추가되며 Inventory 크기만큼 InventorySystem을 통해 슬롯을 생성하며 각 슬롯을 관리
/// </summary>
[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _inventorySize;

    [SerializeField] private InventorySystem _inventorySystem;
    public InventorySystem InventorySystem => _inventorySystem;

    // public static UnityAction<InventorySystem> OnUpdateInventoryUI;

    private void Awake() => _inventorySystem = new InventorySystem(_inventorySize);
}