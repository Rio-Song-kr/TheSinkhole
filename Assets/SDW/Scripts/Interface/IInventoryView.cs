/// <summary>
/// 인벤토리 UI 표시를 위한 인터페이스
/// </summary>
public interface IInventoryView
{
    /// <summary>
    /// 지정된 슬롯의 표시를 업데이트
    /// </summary>
    /// <param name="slotIndex">업데이트할 슬롯 인덱스</param>
    /// <param name="slot">표시할 슬롯 데이터</param>
    void UpdateSlotDisplay(int slotIndex, InventorySlot slot);

    /// <summary>
    /// 지정된 슬롯의 표시를 지움
    /// </summary>
    /// <param name="slotIndex">지울 슬롯 인덱스</param>
    void ClearSlotDisplay(int slotIndex);
}