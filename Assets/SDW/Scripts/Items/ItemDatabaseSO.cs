using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 생성된 Item을 등록하여 관리하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Item Database")]
public class ItemDatabaseSO : ScriptableObject
{
    public GameObject[] ModelPrefabObjects;

    public void OnSetPrefab(ref ItemDataSO itemDataSO)
    {
        foreach (var model in ModelPrefabObjects)
        {
            if (!model.name.Equals(itemDataSO.ItemEnName.ToString())) continue;

            itemDataSO.ModelPrefab = model;
        }

        if (itemDataSO.ModelPrefab.Equals(null))
            Debug.LogWarning($"{itemDataSO.ItemEnName}과 일치하는 프리팹이 없습니다.");
    }
}