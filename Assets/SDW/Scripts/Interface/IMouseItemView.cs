using UnityEngine;

/// <summary>
/// 마우스 포인터를 따라다니는 아이템 표시를 위한 인터페이스
/// 드래그 앤 드롭 기능에서 사용되는 마우스 아이템의 표시 및 관리를 담당
/// </summary>
public interface IMouseItemView
{
    /// <summary>
    /// 마우스에 아이템을 표시
    /// </summary>
    /// <param name="slot">표시할 아이템 슬롯</param>
    void ShowItem(InventorySlot slot);

    /// <summary>
    /// 마우스의 아이템 표시를 제거
    /// </summary>
    void ClearItem();

    /// <summary>
    /// 마우스 위치에 따라 아이템 표시 위치를 업데이트
    /// </summary>
    void UpdatePosition();

    /// <summary>
    /// 현재 마우스에 있는 아이템을 가져옴
    /// </summary>
    /// <returns>현재 마우스 아이템 슬롯</returns>
    InventorySlot GetCurrentItem();

    /// <summary>
    /// 마우스에 아이템이 있는지 확인
    /// </summary>
    /// <returns>마우스에 아이템이 있으면 true</returns>
    bool HasItem();
    public bool IsInsideInventoryArea(Vector2 screenPosition);
}