using UnityEngine;

public class TestMonsterSpawn : MonoBehaviour
{
    private void Start()
    {
        var spider = GameManager.Instance.Monster.MonsterPools[MonsterEnName.BabySpider].Pool.Get();
        spider.transform.parent = transform;
        spider.transform.position = Vector3.right * 2f + Vector3.up * 0.5f;

        var worm = GameManager.Instance.Monster.MonsterPools[MonsterEnName.LittleWorm].Pool.Get();
        worm.transform.parent = transform;
        worm.transform.position = Vector3.right * 4f + Vector3.up * 0.5f;
    }
}