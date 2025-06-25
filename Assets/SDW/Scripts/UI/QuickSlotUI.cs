using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI 슬롯들을 인벤토리 데이터와 연결하는 일종의 바인딩을 위한 클래스
/// </summary>
public class QuickSlotUI : InventoryDisplay
{
    [SerializeField] private Inventory m_inventory;
    private InventorySlotUI[] m_slots;

    /// <summary>
    /// QuickSlotUI 하위의 모든 InventorySlotUI 컴포넌트를 연결
    /// </summary>
    private void Awake() => m_slots = GetComponentsInChildren<InventorySlotUI>();

    /// <summary>
    /// 인벤토리가 존재하는지 여부를 확인하고 QuickSlot과 인벤토리를 연결
    /// 연결되는 인벤토리는 플레이어에 추가된 인벤토리임
    /// </summary>
    public override void Start()
    {
        base.Start();

        if (m_inventory != null)
        {
            m_inventorySystem = m_inventory.InventorySystem;
            m_inventorySystem.OnSlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"{gameObject} : 인벤토리가 정의되지 않았습니다.");

        AssignSlot(m_inventorySystem);
    }

    /// <summary>
    /// 슬롯의 수와 인벤토리의 크기를 확인하고, 슬롯 관리를 위한 dictionary에 추가
    /// 또한, QuickSlotUI의 개별 slot을 Player에 추가된 인벤토리의 슬롯으로 초기화
    /// </summary>
    /// <param name="inventory"></param>
    public override void AssignSlot(InventorySystem inventory)
    {
        m_slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (m_slots.Length != m_inventorySystem.InventorySize)
            Debug.Log($"{gameObject} : 인벤토리의 크기와 슬롯의 개수가 다릅니다.");

        for (int i = 0; i < m_inventorySystem.InventorySize; i++)
        {
            m_slotDictionary.Add(m_slots[i], m_inventorySystem.InventorySlots[i]);
            m_slots[i].Init(m_inventorySystem.InventorySlots[i]);
        }
    }
}