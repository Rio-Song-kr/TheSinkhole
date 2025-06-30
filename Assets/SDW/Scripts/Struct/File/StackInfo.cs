using System;

/// <summary>
/// 스택 정보를 담기 위한 구조체
/// </summary>
[Serializable]
public struct StackInfo
{
    public InventorySlot slot;
    public InventorySystem inventorySystem;
    public int currentAmount;
}