using System;
using UnityEngine;

/// <summary>
/// 개별 슬롯 데이터를 위한 클래스
/// Inventory와 관련된 모델(데이터 계층) 역할
/// </summary>
[Serializable]
public class InventorySlot
{
    [SerializeField] private ItemDataSO m_itemDataSO;
    public ItemDataSO ItemDataSO => m_itemDataSO;

    [SerializeField] private int m_itemCount;
    public int ItemCount => m_itemCount;

    /// <summary>
    /// Item Slot이 비어있는 경우(초기 상태), Slot은 존재하기에 아이콘 등을 표시하지 않기 위해 사용
    /// </summary>
    public InventorySlot()
    {
        ClearSlot();
    }

    /// <summary>
    /// Inventory의 Slot에 추가되는 아이템과 수량을 설정
    /// </summary>
    /// <param name="item">해당 슬롯에 표시될 아이템</param>
    /// <param name="amount">해당 슬롯에 추가되는 아이템의 초기 수량</param>
    public InventorySlot(ItemDataSO item, int amount)
    {
        m_itemDataSO = item;
        m_itemCount = amount;
    }

    /// <summary>
    /// Item을 버리거나, 모두 사용/파괴될 때 슬롯을 비우기 위해 사용
    /// </summary>
    public void ClearSlot()
    {
        m_itemDataSO = null;
        m_itemCount = -1;
    }

    /// <summary>
    /// 해당 수량만큼 아이템을 추가할 수 있는지 여부와 추가 가능한 수량이 얼마나 남았는지 확인
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    /// <param name="amountRemaining">추가 가능한 수량</param>
    /// <returns>추가 가능 여부</returns>
    public bool CanAdd(int amount, out int amountRemaining)
    {
        amountRemaining = m_itemDataSO.ItemMaxOwn - m_itemCount;

        return amountRemaining != 0;
    }

    /// <summary>
    /// 기존 보유 수량과 추가하려는 수량을 더해 MaxStackSize를 넘기는지 여부를 판단
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    /// <returns>해당 아이템이 최대 수량보다 적으면 true, 그렇지 않으면 false</returns>
    public bool CanAdd(int amount) => m_itemCount + amount <= m_itemDataSO.ItemMaxOwn;

    /// <summary>
    /// Amount만큼 아이템 추가
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    public void AddItem(int amount) => m_itemCount += amount;

    /// <summary>
    /// 동일한 아이템인지 확인하여 기존 아이템에 추가하거나, 다르다면 새로운 Slot(아이템)을 추가
    /// </summary>
    /// <param name="slot">추가할 슬롯(아이템)</param>
    public void AddItem(InventorySlot slot)
    {
        if (m_itemDataSO == slot.ItemDataSO) AddItem(slot.ItemCount);
        else
        {
            m_itemDataSO = slot.ItemDataSO;
            m_itemCount = 0;
            AddItem(slot.ItemCount);
        }
    }

    /// <summary>
    /// 빈 슬롯에 Item을 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    public void AddItemToEmptySlot(ItemDataSO item, int amount)
    {
        m_itemDataSO = item;
        m_itemCount = amount;
    }

    /// <summary>
    /// Amount만큼 아이템 제거
    /// </summary>
    /// <param name="amount">제거할 수량</param>
    public void RemoveItem(int amount)
    {
        m_itemCount -= amount;

        if (m_itemCount <= 0) ClearSlot();
    }

    /// <summary>
    /// 인벤토리의 슬롯 아이템을 분할
    /// 현재 슬롯을 Shift + Click하면 절반으로 나눠짐
    /// </summary>
    /// <param name="slot">분할할 슬롯</param>
    /// <returns>분할이 가능하면 true, 1개 이하라 분할이 불가능하면 false</returns>
    public bool SplitItemAmount(out InventorySlot slot)
    {
        if (ItemCount <= 1)
        {
            slot = null;
            return false;
        }

        int halfAmount = Mathf.RoundToInt(ItemCount / 2);
        RemoveItem(halfAmount);

        slot = new InventorySlot(m_itemDataSO, halfAmount);
        return true;
    }
}