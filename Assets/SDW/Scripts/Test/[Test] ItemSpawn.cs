using UnityEngine;

public class TestItemSpawn : MonoBehaviour
{
    private Vector3 m_position = Vector3.zero;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var herb = GameManager.Instance.Item.ItemPools["허브 알약"].Pool.Get();
            herb.transform.parent = transform.parent;
            herb.ItemAmount = 500;
            m_position += Vector3.forward * 2f;
            herb.transform.position = m_position;
        }

        m_position += Vector3.right * 2f;

        for (int i = 0; i < 10; i++)
        {
            var shovel = GameManager.Instance.Item.ItemPools["삽"].Pool.Get();
            shovel.transform.parent = transform.parent;
            shovel.ItemAmount = 500;
            m_position -= Vector3.forward * 2f;
            shovel.transform.position = m_position;
        }
    }
}