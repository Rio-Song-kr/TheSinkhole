using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 생성된 Item을 등록하여 관리하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Item Database")]
public class ItemPrefabDatabaseSO : ScriptableObject
{
    public GameObject[] ModelPrefabObjects;
    public Sprite[] IconSprites;

    public void OnSetPrefab(ref ItemDataSO itemDataSO)
    {
        for (int i = 0; i < ModelPrefabObjects.Length; i++)
        {
            if (!ModelPrefabObjects[i].name.Equals(itemDataSO.ItemEnName.ToString())) continue;

            itemDataSO.ModelPrefab = ModelPrefabObjects[i];

            if (!ModelPrefabObjects[i].name.Equals(IconSprites[i].name)) continue;
            itemDataSO.Icon = IconSprites[i];
        }

        if (itemDataSO.ModelPrefab.Equals(null))
            Debug.LogWarning($"{itemDataSO.ItemEnName}과 일치하는 프리팹이 없습니다.");
    }
}