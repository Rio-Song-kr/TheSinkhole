using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public ItemType ItemType;
    [TextArea(4, 4)] public string ItemText;
    public int ItemMaxOwn;

    [Header("2D Icon")]
    public Sprite Icon;

    [Header("Model")]
    public GameObject ModelPrefab;

    [Header("Data")]
    public Item ItemData = new Item();

    public bool IsStackable => ItemMaxOwn > 1;

    private void OnValidate()
    {
        if (ItemMaxOwn < 1) ItemMaxOwn = 1;
        if (string.IsNullOrEmpty(ItemData.ItemName)) ItemData.ItemName = name;
    }

    public Item CreateItem()
    {
        var item = new Item(this);
        return item;
    }
}