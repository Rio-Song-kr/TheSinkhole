using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemDataSO _itemDataSO;
    public ItemDataSO ItemDataSo => _itemDataSO;

    [SerializeField] private int _itemCount;
    public int ItemCount => _itemCount;

    /// <summary>
    /// Item Slot이 비어있는 경우(초기 상태), Slot은 존재하지만 아이콘 등을 표시하지 않기 위해 사용
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
        _itemDataSO = item;
        _itemCount = amount;
    }

    /// <summary>
    /// Item을 버리거나, 모두 사용/파괴될 때 슬롯을 비우기 위해 사용
    /// </summary>
    private void ClearSlot()
    {
        _itemDataSO = null;
        _itemCount = -1;
    }

    /// <summary>
    /// 해당 수량만큼 아이템을 추가할 수 있는지 여부와 추가 가능한 수량이 얼마나 남았는지 확인
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    /// <param name="amountRemaining">추가 가능한 수량</param>
    /// <returns>추가 가능 여부</returns>
    public bool CanAdd(int amount, out int amountRemaining)
    {
        // amountRemaining = _itemData.MaxItemCount - _itemCount;
        amountRemaining = _itemDataSO.ItemMaxOwn - _itemCount;
        // return amountRemaining >= amount;
        return amountRemaining != 0;
    }

    /// <summary>
    /// 기존 보유 수량과 추가하려는 수량을 더해 MaxStackSize를 넘기는지 여부를 판단
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    /// <returns>해당 아이템이 최대 수량보다 적으면 true, 그렇지 않으면 false</returns>
    public bool CanAdd(int amount) => _itemCount + amount <= _itemDataSO.ItemMaxOwn;

    /// <summary>
    /// Amount만큼 아이템 추가
    /// </summary>
    /// <param name="amount">추가할 수량</param>
    public void AddItem(int amount) => _itemCount += amount;

    /// <summary>
    /// 빈 슬롯에 Item을 추가
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    /// <param name="amount">추가할 수량</param>
    public void AddItemToEmptySlot(ItemDataSO item, int amount)
    {
        _itemDataSO = item;
        _itemCount = amount;
    }

    /// <summary>
    /// Amount만큼 아이템 제거
    /// </summary>
    /// <param name="amount">제거할 수량</param>
    public void RemoveItem(int amount)
    {
        _itemCount -= amount;

        if (_itemCount <= 0) ClearSlot();
    }
}