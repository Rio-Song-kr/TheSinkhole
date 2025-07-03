using UnityEngine;
using UnityEngine.AI;

public class TestMonsterSpawn : MonoBehaviour
{
    private void Start()
    {
        var spider = GameManager.Instance.Monster.MonsterPools[MonsterEnName.BabySpider].Pool.Get();
        spider.Initialize();
        spider.transform.parent = transform;
        spider.transform.position = Vector3.right * 27f + Vector3.up * 0.5f + Vector3.forward * 2f;
        spider.StartTrace();

        // var worm = GameManager.Instance.Monster.MonsterPools[MonsterEnName.LittleWorm].Pool.Get();
        // worm.Initialize();
        // worm.transform.parent = transform;
        // worm.transform.position = Vector3.right * 27f + Vector3.up * 0.5f + Vector3.back * 2f;
        // worm.StartTrace();
    }
}