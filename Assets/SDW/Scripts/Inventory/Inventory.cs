using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 인벤토리 컨테이너 역할
/// Player에 추가되며 Inventory 크기만큼 InventorySystem을 통해 슬롯을 생성하며 각 슬롯을 관리
/// Inventory와 관련된 모델(데이터 계층) 역할
/// </summary>
[Serializable]
public class Inventory : MonoBehaviour, ISaveable
{
    [Header("Save Settings")]
    [SerializeField] private string m_saveID;
    [SerializeField] private ObjectType m_objectType;

    //# 비고 : 현재는 dynamicInventorySlot이라 사용하고 있지만, 추후 상자가 추가될 시 상자와 백팩은 변수명은 분리하여 구분
    [Header("Set Slot Size")]
    [SerializeField] private int m_dynamicInventorySlotSize;
    [SerializeField] private int m_quickSlotSize;

    //# 하단의 Quick Slot용 Inventory System
    [Header("Inventory System")]
    [SerializeField] private InventorySystem m_quickSlotInventorySystem;
    public InventorySystem QuickSlotInventorySystem => m_quickSlotInventorySystem;

    //# 화면 중앙에 표시되는 Backpack, 상자 등의 InventorySystem, 추후 상자 추가시 변수명은 구분해야 함
    [SerializeField] private InventorySystem m_dynamicInventorySystem;
    public InventorySystem DynamicInventorySystem => m_dynamicInventorySystem;

    public static Action<InventorySystem, bool> OnDynamicDisplayRequest;
    public static Action<int> OnSelectedItemChanged;

    private ItemEnName m_selectedItemEnName;
    private ToolType m_quickSlotItemToolType = ToolType.None;
    private ItemType m_quickSlotItemType = ItemType.None;
    private ItemDataSO m_quickSlotItemData;

    /// <summary>
    /// 저장을 위한 Id 케ㅡ
    /// 인벤토리 시스템들을 size 만큼 초기화
    /// </summary>
    private void Awake()
    {
        if (m_objectType == ObjectType.SceneStatic)
        {
            if (string.IsNullOrEmpty(m_saveID))
            {
                Debug.LogError($"{gameObject.name}의 Save ID가 설정되지 않았습니다.");
                return;
            }
        }
        else
        {
            if (string.IsNullOrEmpty(m_saveID))
                m_saveID = Guid.NewGuid().ToString();
        }
        m_quickSlotInventorySystem = new InventorySystem(m_quickSlotSize);
        m_dynamicInventorySystem = new InventorySystem(m_dynamicInventorySlotSize);
    }

    /// <summary>
    /// 현재 B키 입력 시 Dynamic Inventory Open/Close
    /// </summary>
    public void OnInventoryKeyPressed()
    {
        OnDynamicDisplayRequest?.Invoke(m_dynamicInventorySystem, !DynamicUIController.IsOpened);

        if (DynamicUIController.IsOpened) GameManager.Instance.UI.SetCursorUnlock();
        else GameManager.Instance.UI.SetCursorLock();
    }

    /// <summary>
    /// Escape 키 입력 시 Dynamic Inventory Close
    /// </summary>
    public void OnCloseKeyPressed()
    {
        OnDynamicDisplayRequest?.Invoke(m_dynamicInventorySystem, false);
        GameManager.Instance.UI.SetCursorLock();
    }

    /// <summary>
    /// 넘패드 1~0 키 입력을 확인하여 퀵슬롯 선택 처리
    /// 0키는 인덱스 9로, 1~9키는 인덱스 0~8로 매핑
    /// </summary>
    public void OnNumpadKeyPressed(InputAction.CallbackContext ctx)
    {
        int m_selectedIndex = int.Parse(ctx.control.name);

        m_selectedIndex = m_selectedIndex == 0 ? 9 : m_selectedIndex - 1;
        SelectQuickSlot(m_selectedIndex);
    }

    /// <summary>
    /// 지정된 인덱스의 퀵슬롯을 선택
    /// 영문 이름과 Tool type관련 처리
    /// </summary>
    /// <param name="selectedIndex">선택할 슬롯 인덱스</param>
    private void SelectQuickSlot(int selectedIndex)
    {
        if (m_quickSlotInventorySystem.InventorySlots[selectedIndex].ItemDataSO == null)
        {
            OnSelectedItemChanged?.Invoke(selectedIndex);
            return;
        }

        m_quickSlotItemData = m_quickSlotInventorySystem.InventorySlots[selectedIndex].ItemDataSO;
        m_selectedItemEnName = m_quickSlotItemData.ItemEnName;
        m_quickSlotItemType = m_quickSlotItemData.ItemType;

        switch (m_selectedItemEnName)
        {
            case ItemEnName.Hammer:
                m_quickSlotItemToolType = ToolType.Hammer;
                break;
            case ItemEnName.Shovel:
                m_quickSlotItemToolType = ToolType.Shovel;
                break;
            case ItemEnName.Pick:
                m_quickSlotItemToolType = ToolType.Pick;
                break;
            case ItemEnName.Pail:
                m_quickSlotItemToolType = ToolType.Water;
                break;
            default:
                m_quickSlotItemToolType = ToolType.None;
                break;
        }

        OnSelectedItemChanged?.Invoke(selectedIndex);
    }

    /// <summary>
    /// 현재 선택된 아이템의 영문명을 반환
    /// </summary>
    /// <returns>선택된 아이템의 영문명</returns>
    public ItemEnName GetItemName() => m_selectedItemEnName;

    /// <summary>
    /// 현재 선택된 아이템의 ToolType을 반환
    /// </summary>
    /// <returns>현재 선택된 아이템의 ToolType을 반환</returns>
    public ToolType GetItemToolType() => m_quickSlotItemToolType;

    /// <summary>
    /// 현재 선택된 아이템의 ItemType을 반환
    /// </summary>
    /// <returns>현재 선택된 아이템의 ItemType을 반환</returns>
    public ItemType GetQuickSlotItemType() => m_quickSlotItemType;

    /// <summary>
    /// 현재 선택된 아이템의 ItemDataSO를 반환
    /// </summary>
    /// <returns>현재 선택된 아이템의 ItemDataSO를 반환</returns>
    public ItemDataSO GetQuickSlotItemData() => m_quickSlotItemData;

    /// <summary>
    /// ItemEnName 아이템이 총 몇 개 있는지 반환
    /// </summary>
    /// <param name="itemEnName">찾을 아이템의 영어 이름</param>
    /// <returns>찾은 아이템의 수를 반환</returns>
    public int GetItemAmounts(ItemEnName itemEnName)
    {
        int itemAmounts = 0;

        foreach (var slot in m_quickSlotInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null) continue;
            if (slot.ItemDataSO.ItemEnName == itemEnName) itemAmounts += slot.ItemCount;
        }

        foreach (var slot in m_dynamicInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null) continue;
            if (slot.ItemDataSO.ItemEnName == itemEnName) itemAmounts += slot.ItemCount;
        }

        return itemAmounts;
    }

    /// <summary>
    /// 비어있는 Slot의 수를 반환
    /// </summary>
    /// <returns>비어있는 Slot의 수</returns>
    public int GetRemainingSlots()
    {
        int slotAmounts = 0;

        foreach (var slot in m_quickSlotInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null)
                slotAmounts++;
        }

        foreach (var slot in m_dynamicInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null)
                slotAmounts++;
        }

        return slotAmounts;
    }

    /// <summary>
    /// 특정 아이템을 제거
    /// </summary>
    /// <param name="itemEnName">제거하려는 아이템 영어 이름</param>
    /// <param name="amount">제거하려는 아이템의 수</param>
    /// <returns>제거가 완료되면 true, 완료하지 못하면 false를 반환</returns>
    public bool RemoveItemAmounts(ItemEnName itemEnName, int amount)
    {
        //todo 인벤토리 순회하면서 개수 채크
        int remainingAmounts = GetItemAmounts(itemEnName);

        if (amount > remainingAmounts) return false;

        //# 1. 아이템 수가 충분한 경우, 그냥 슬롯 하나에서 처리
        foreach (var slot in m_quickSlotInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null) continue;
            if (slot.ItemDataSO.ItemEnName == itemEnName)
            {
                //# 슬롯에 있는 아이템의 수가 제거하려는 수보다 크거나 같을 때
                if (slot.ItemCount >= amount)
                {
                    slot.RemoveItem(amount);
                    m_quickSlotInventorySystem.OnSlotChanged?.Invoke(slot);
                    break;
                }
                //# 슬롯에 있는 수가 제거하려는 수보다 작은 경우
                amount -= slot.ItemCount;
                slot.RemoveItem(slot.ItemCount);
                m_quickSlotInventorySystem.OnSlotChanged?.Invoke(slot);
            }
        }
        if (amount == 0) return true;

        foreach (var slot in m_dynamicInventorySystem.InventorySlots)
        {
            if (slot.ItemDataSO == null) continue;
            if (slot.ItemDataSO.ItemEnName == itemEnName)
            {
                //# 슬롯에 있는 아이템의 수가 제거하려는 수보다 크거나 같을 때
                if (slot.ItemCount >= amount)
                {
                    slot.RemoveItem(amount);
                    m_dynamicInventorySystem.OnSlotChanged?.Invoke(slot);
                    break;
                }
                //# 슬롯에 있는 수가 제거하려는 수보다 작은 경우
                amount -= slot.ItemCount;
                slot.RemoveItem(slot.ItemCount);
                m_dynamicInventorySystem.OnSlotChanged?.Invoke(slot);
            }
        }

        return true;
    }

    /// <summary>
    /// 마인크래프트 스타일의 아이템 추가 방식
    /// 인벤토리 타입에 관계없이 기존 스택을 우선으로 채우고, 이후 빈 슬롯에 추가
    /// </summary>
    /// <param name="itemData">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>추가되지 못한 남은 수량</returns>
    public int AddItemSmart(ItemDataSO itemData, int amount)
    {
        int remainingAmount = amount;

        //# 모든 인벤토리에서 기존 스택들을 수집하고 수량순으로 정렬
        var existingStacks = GetAllExistingStacks(itemData);

        //# 기존 스택들을 우선으로 채우기 (수량이 많은 순서대로)
        foreach (var stackInfo in existingStacks)
        {
            if (stackInfo.slot.CanAdd(remainingAmount, out int canAddAmount))
            {
                int addAmount = Mathf.Min(remainingAmount, canAddAmount);
                stackInfo.slot.AddItem(addAmount);
                remainingAmount -= addAmount;

                //@ 해당 인벤토리 시스템에 변경 알림
                stackInfo.inventorySystem.NotifySlotChanged(stackInfo.slot);

                if (remainingAmount == 0) return 0;
            }
        }

        //# 빈 슬롯에 추가 (QuickSlot 우선)
        if (remainingAmount > 0) remainingAmount = AddToEmptySlots(itemData, remainingAmount);

        return remainingAmount;
    }

    /// <summary>
    /// 모든 인벤토리에서 지정된 아이템의 기존 스택들을 수집하고 정렬
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>수량순으로 정렬된 스택 정보 리스트</returns>
    private List<StackInfo> GetAllExistingStacks(ItemDataSO itemData)
    {
        var allStacks = new List<StackInfo>();

        //# QuickSlot에서 기존 스택 찾기
        if (m_quickSlotInventorySystem.FindItemSlots(itemData, out var quickSlotStacks))
        {
            foreach (var slot in quickSlotStacks)
            {
                allStacks.Add(new StackInfo
                {
                    slot = slot,
                    inventorySystem = m_quickSlotInventorySystem,
                    currentAmount = slot.ItemCount
                });
            }
        }

        //# Dynamic에서 기존 스택 찾기
        if (m_dynamicInventorySystem.FindItemSlots(itemData, out var dynamicStacks))
        {
            foreach (var slot in dynamicStacks)
            {
                allStacks.Add(new StackInfo
                {
                    slot = slot,
                    inventorySystem = m_dynamicInventorySystem,
                    currentAmount = slot.ItemCount
                });
            }
        }

        //# 수량이 많은 순서대로 정렬
        allStacks.Sort((a, b) => b.currentAmount.CompareTo(a.currentAmount));

        return allStacks;
    }

    /// <summary>
    /// 빈 슬롯들에 아이템을 추가 (QuickSlot 우선)
    /// </summary>
    /// <param name="itemData">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    /// <returns>남은 수량</returns>
    private int AddToEmptySlots(ItemDataSO itemData, int amount)
    {
        int remainingAmount = amount;

        //# QuickSlot 빈 슬롯 우선
        while (remainingAmount > 0 && m_quickSlotInventorySystem.GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, itemData.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(itemData, addAmount);
            remainingAmount -= addAmount;

            m_quickSlotInventorySystem.NotifySlotChanged(emptySlot);
        }

        //# Dynamic 빈 슬롯
        while (remainingAmount > 0 && m_dynamicInventorySystem.GetEmptySlot(out var emptySlot))
        {
            int addAmount = Mathf.Min(remainingAmount, itemData.ItemMaxOwn);
            emptySlot.AddItemToEmptySlot(itemData, addAmount);
            remainingAmount -= addAmount;

            m_dynamicInventorySystem.NotifySlotChanged(emptySlot);
        }

        return remainingAmount;
    }

    public string GetUniqueID() => m_saveID;

    /// <summary>
    /// Save를 위한 데이터를 읽어오기 위한 메서드(프로토타입)
    /// </summary>
    /// <returns>InventorySaveData를 반환</returns>
    public object GetSaveData() => new InventorySaveData
    {
        SaveID = m_saveID,
        Type = m_objectType,
        QuickSlotInventorySystem = m_quickSlotInventorySystem,
        DynamicInventorySystem = m_dynamicInventorySystem,
        SelectedItemEnName = m_selectedItemEnName,
        ToolType = m_quickSlotItemToolType
    };

    /// <summary>
    /// 저장된 데이터로부터 Inventory 데이터를 할당(프로토타입)
    /// </summary>
    /// <param name="data">Inventory에 할당하기 위한 데이터</param>
    public void LoadSaveDta(object data)
    {
        var inventoryData = (InventorySaveData)data;
        m_objectType = inventoryData.Type;
        m_quickSlotInventorySystem = inventoryData.QuickSlotInventorySystem;
        m_dynamicInventorySystem = inventoryData.DynamicInventorySystem;
        m_selectedItemEnName = inventoryData.SelectedItemEnName;
        m_quickSlotItemToolType = inventoryData.ToolType;
    }

    /// <summary>
    /// Save/Load 시 Scene 기본 생성 Object의 ID를 생성하기 위한 메서드
    /// </summary>
#if UNITY_EDITOR
    [ContextMenu("Generate Scene ID")]
    public void GenerateSceneID()
    {
        if (m_objectType == ObjectType.SceneStatic)
        {
            m_saveID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"Scene ID 생성: {m_saveID}");
        }
    }
#endif
}