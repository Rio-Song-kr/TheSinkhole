/// <summary>
/// Save/Load
/// </summary>
[System.Serializable]
public class Item
{
    public int ItemId;
    public string ItemName;
    public ItemEffect[] ItemEffects;

    public Item()
    {
        ItemId = -1;
        ItemName = "";
    }

    public Item(ItemDataSO itemDataSO)
    {
        ItemName = itemDataSO.name;
        ItemId = itemDataSO.ItemData.ItemId;

        ItemEffects = new ItemEffect[itemDataSO.ItemData.ItemEffects.Length];

        for (int i = 0; i < ItemEffects.Length; i++)
        {
            ItemEffects[i] = new ItemEffect(
                itemDataSO.ItemData.ItemEffects[i].Status,
                itemDataSO.ItemData.ItemEffects[i].EffectAmount
            );
        }
    }
}