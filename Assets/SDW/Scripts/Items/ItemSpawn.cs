using System;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject[] InitialItemPostions;

    private void Start()
    {
        foreach (var itemPosition in InitialItemPostions)
        {
            if (Enum.TryParse<ItemEnName>(itemPosition.name, true, out var itemEnName))
            {
                var item = GameManager.Instance.Item.ItemPools[itemEnName].Pool.Get();
                item.transform.parent = transform;
                item.ItemAmount = 1;
                item.transform.position = itemPosition.transform.position;
            }
        }
    }
}