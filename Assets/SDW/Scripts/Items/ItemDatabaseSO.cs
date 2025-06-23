using UnityEngine;

/// <summary>
/// 생성된 Item을 등록하여 관리하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public ItemDataSO[] ItemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].ItemData.ItemId = i;
        }
    }
}