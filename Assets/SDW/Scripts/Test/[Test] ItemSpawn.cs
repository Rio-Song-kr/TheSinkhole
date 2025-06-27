using UnityEngine;

public class TestItemSpawn : MonoBehaviour
{
    private Vector3 m_position = Vector3.up * 0.5f;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var herb = GameManager.Instance.Item.ItemPools["허브 알약"].Pool.Get();
            herb.ItemAmount = 1000;
            m_position += Vector3.forward * 2f;
            herb.transform.position = m_position;
        }
    }
}