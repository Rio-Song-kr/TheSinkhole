using UnityEngine;

/// <summary>
/// 동적 인벤토리 UI의 표시/숨김을 제어하는 컨트롤러 클래스
/// Inventory 클래스의 이벤트를 구독하여 백팩, 상자 등의 동적 인벤토리 UI를 관리
/// 인벤토리 패널, 아이템 정보 영역 등의 UI 요소들을 통합적으로 제어
/// </summary>
public class DynamicUIController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryUiContainer;
    [SerializeField] private DynamicInventoryView _inventoryPanel;

    /// <summary>
    /// 컴포넌트 초기화 시 모든 UI 요소를 비활성화 상태로 설정
    /// </summary>
    private void Awake() => HandleCloseInventory();

    /// <summary>
    /// 인벤토리 UI를 닫고 모든 관련 UI 요소들을 비활성화
    /// </summary>
    private void HandleCloseInventory() => _inventoryUiContainer.SetActive(false);

    /// <summary>
    /// 컴포넌트 활성화 시 Inventory의 동적 표시 요청 이벤트를 구독
    /// </summary>
    private void OnEnable() => Inventory.OnDynamicDisplayRequest += HandleDisplayInventory;

    /// <summary>
    /// 컴포넌트 비활성화 시 Inventory의 동적 표시 요청 이벤트 구독 해제
    /// </summary>
    private void OnDisable() => Inventory.OnDynamicDisplayRequest -= HandleDisplayInventory;

    /// <summary>
    /// 인벤토리 표시 상태에 따라 UI 요소들을 활성화/비활성화
    /// 인벤토리가 열릴 때 해당 인벤토리 시스템으로 뷰를 새로고침
    /// </summary>
    /// <param name="inventorySystem">표시할 인벤토리 시스템</param>
    /// <param name="isOpen">인벤토리 열림 상태</param>
    private void HandleDisplayInventory(InventorySystem inventorySystem, bool isOpen)
    {
        _inventoryUiContainer.SetActive(isOpen);

        if (isOpen) _inventoryPanel.RefreshDisplaySlots(inventorySystem);
    }
}