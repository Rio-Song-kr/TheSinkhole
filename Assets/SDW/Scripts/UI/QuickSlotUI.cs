using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuickSlotUI : InventoryDisplay
{
    [SerializeField] private Inventory m_inventory;
    private InventorySlotUI[] m_slots;

    private void Awake()
    {
        m_slots = GetComponentsInChildren<InventorySlotUI>();
    }

    public override void Start()
    {
        base.Start();

        if (m_inventory != null)
        {
            m_inventorySystem = m_inventory.MInventorySystem;
            m_inventorySystem.OnSlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {gameObject}");

        AssignSlot(m_inventorySystem);
    }

    public override void AssignSlot(InventorySystem inventory)
    {
        m_slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (m_slots.Length != m_inventorySystem.InventorySize) Debug.Log($"Inventory slots out of sync on {gameObject}");

        for (int i = 0; i < m_inventorySystem.InventorySize; i++)
        {
            m_slotDictionary.Add(m_slots[i], m_inventorySystem.InventorySlots[i]);
            m_slots[i].Init(m_inventorySystem.InventorySlots[i]);
        }
    }
}