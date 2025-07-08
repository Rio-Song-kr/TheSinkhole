using System;

/// <summary>
/// Inventory 정보를 저장/불러오기를 위한 구조체(프로토타입)
/// </summary>
[Serializable]
public struct InventorySaveData
{
    public string SaveID;
    public ObjectType Type;
    public InventorySystem QuickSlotInventorySystem;
    public InventorySystem DynamicInventorySystem;
    public ItemEnName SelectedItemEnName;
    public ToolType ToolType;
}