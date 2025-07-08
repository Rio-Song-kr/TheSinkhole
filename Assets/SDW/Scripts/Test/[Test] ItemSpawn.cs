using System;
using UnityEngine;

public class TestItemSpawn : MonoBehaviour
{
    private Vector3 m_position = Vector3.forward * 15;

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


        var shovel = GameManager.Instance.Item.ItemPools[ItemEnName.Shovel].Pool.Get();
        shovel.transform.parent = transform;
        shovel.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        shovel.transform.position = m_position;

        var hammer = GameManager.Instance.Item.ItemPools[ItemEnName.Hammer].Pool.Get();
        hammer.transform.parent = transform;
        hammer.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        hammer.transform.position = m_position;

        var pick = GameManager.Instance.Item.ItemPools[ItemEnName.Pick].Pool.Get();
        pick.transform.parent = transform;
        pick.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        pick.transform.position = m_position;

        var pail = GameManager.Instance.Item.ItemPools[ItemEnName.Pail].Pool.Get();
        pail.transform.parent = transform;
        pail.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        pail.transform.position = m_position;


        var flareGun = GameManager.Instance.Item.ItemPools[ItemEnName.FlareGun].Pool.Get();
        flareGun.transform.parent = transform;
        flareGun.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        flareGun.transform.position = m_position;

        var emptyFlareGun = GameManager.Instance.Item.ItemPools[ItemEnName.EmptyFlareGun].Pool.Get();
        emptyFlareGun.transform.parent = transform;
        emptyFlareGun.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        emptyFlareGun.transform.position = m_position;

        var flareGunBullets = GameManager.Instance.Item.ItemPools[ItemEnName.FlareGunBullets].Pool.Get();
        flareGunBullets.transform.parent = transform;
        flareGunBullets.ItemAmount = 1;
        m_position -= Vector3.right * 2f;
        flareGunBullets.transform.position = m_position;


        m_position = Vector3.forward * 15 + Vector3.right * 20;
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