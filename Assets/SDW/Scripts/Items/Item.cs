/// <summary>
/// Save/Load
/// </summary>
[System.Serializable]
public class Item
{
    public int ItemId;
    public string ItemName;

    public Item()
    {
        ItemId = -1;
        ItemName = "";
    }

    public Item(ItemDataSO itemDataSO)
    {
        ItemName = itemDataSO.name;
        ItemId = itemDataSO.ItemData.ItemId;
    }
}