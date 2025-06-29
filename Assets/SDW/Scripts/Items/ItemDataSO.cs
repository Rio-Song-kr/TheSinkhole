using UnityEngine;

/// <summary>
/// Game에서 사용할 Item의 데이터를 포함하고, Icon, Prefab 등을 정의하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public ItemType ItemType;
    [TextArea(4, 4)] public string ItemText;
    public int ItemMaxOwn;
    public ItemEnName ItemEnName;

    [Header("2D Icon")]
    public Sprite Icon;

    [Header("Model")]
    public GameObject ModelPrefab;

    [Header("Data")]
    public Item ItemData = new Item();

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