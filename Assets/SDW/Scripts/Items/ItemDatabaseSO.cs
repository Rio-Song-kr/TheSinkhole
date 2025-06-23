using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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