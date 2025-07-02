using UnityEngine;

public class TestItemSpawn : MonoBehaviour
{
    private Vector3 m_position = Vector3.left * 4;

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var herb = GameManager.Instance.Item.ItemPools[ItemEnName.Herb].Pool.Get();
            herb.transform.parent = transform;
            herb.ItemAmount = 500;
            m_position += Vector3.forward * 2f;
            herb.transform.position = m_position;
        }

        m_position += Vector3.right * 2f;

        var shovel = GameManager.Instance.Item.ItemPools[ItemEnName.Shovel].Pool.Get();
        shovel.transform.parent = transform;
        shovel.ItemAmount = 1;
        m_position -= Vector3.forward * 2f;
        shovel.transform.position = m_position;

        var hammer = GameManager.Instance.Item.ItemPools[ItemEnName.Hammer].Pool.Get();
        hammer.transform.parent = transform;
        hammer.ItemAmount = 1;
        m_position -= Vector3.forward * 2f;
        hammer.transform.position = m_position;

        var pick = GameManager.Instance.Item.ItemPools[ItemEnName.Pick].Pool.Get();
        pick.transform.parent = transform;
        pick.ItemAmount = 1;
        m_position -= Vector3.forward * 2f;
        pick.transform.position = m_position;
    }
}