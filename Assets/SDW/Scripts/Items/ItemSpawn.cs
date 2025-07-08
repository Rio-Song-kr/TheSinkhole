using System;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    private Vector3 m_position = Vector3.forward * 15;
    public GameObject[] InitialItemPostions;

    private void Start()
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     var herb = GameManager.Instance.Item.ItemPools[ItemEnName.Herb].Pool.Get();
        //     herb.transform.parent = transform;
        //     herb.ItemAmount = 100;
        //     m_position += Vector3.forward * 2f;
        //     herb.transform.position = m_position;
        // }

        m_position += Vector3.right * 2f;

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

        m_position = Vector3.forward * 1f + Vector3.right * 20;
        foreach (ItemEnName enName in Enum.GetValues(typeof(ItemEnName)))
        {
            switch (enName)
            {
                case ItemEnName.None:
                case ItemEnName.Shovel:
                case ItemEnName.Hammer:
                case ItemEnName.Pick:
                case ItemEnName.Pail:
                case ItemEnName.FlareGun:
                case ItemEnName.EmptyFlareGun:
                case ItemEnName.FlareGunBullets:
                    continue;
            }

            var item = GameManager.Instance.Item.ItemPools[enName].Pool.Get();
            item.transform.parent = transform;

            if (enName == ItemEnName.Tobacco)
                item.ItemAmount = 10;
            else
                item.ItemAmount = 99;
            m_position -= Vector3.right * 1f;
            item.transform.position = m_position;
        }
    }
}