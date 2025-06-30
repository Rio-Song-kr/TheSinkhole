using System;

[Serializable]
public struct InventorySaveData
{
    public string SaveID;
    public ObjectType Type;
    public InventorySystem QuickSlotInventorySystem;
    public InventorySystem DynamicInventorySystem;
    public ItemEnName SelectedItemEnName;
    public ToolType ToolType;
    public int ItemAmounts;
}