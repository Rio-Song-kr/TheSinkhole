using UnityEngine;

/// <summary>
/// 인벤토리 관련 마우스 입력을 처리하는 핸들러 클래스
/// 인벤토리 영역 밖에서의 클릭 시 아이템 삭제 등의 기능을 담당
/// 좌클릭 시 마우스가 인벤토리 영역 밖에 있으면 들고 있던 아이템을 제거
/// </summary>
public class InventoryInputHandler
{
    private IMouseItemView m_mouseItemView;

    /// <summary>
    /// InventoryInputHandler 생성자
    /// </summary>
    /// <param name="mouseItemView">마우스 아이템 뷰 인터페이스</param>
    public InventoryInputHandler(IMouseItemView mouseItemView)
    {
        m_mouseItemView = mouseItemView;
    }

    /// <summary>
    /// 마우스 입력을 처리
    /// 인벤토리 영역 밖에서 마우스로 아이템을 들고 있을 때 클릭하면 아이템을 삭제
    /// </summary>
    public void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool mouseEmpty = !m_mouseItemView.HasItem();
            bool isInside = m_mouseItemView.IsInsideInventoryArea(Input.mousePosition);

            if (!mouseEmpty && !isInside) m_mouseItemView.ClearItem();
        }
    }
}