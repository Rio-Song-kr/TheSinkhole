using UnityEngine;
using UnityEngine.AI;

public class TestMonsterSpawn : MonoBehaviour
{
    private void Start()
    {
        var spider = GameManager.Instance.Monster.MonsterPools[MonsterEnName.Spider].Pool.Get();
        spider.Initialize();
        spider.transform.parent = transform;
        spider.transform.position = Vector3.right * 27f + Vector3.up * 0.5f + Vector3.left * 2f;
        spider.StartTrace();

        var babySpider = GameManager.Instance.Monster.MonsterPools[MonsterEnName.BabySpider].Pool.Get();
        babySpider.Initialize();
        babySpider.transform.parent = transform;
        babySpider.transform.position = Vector3.right * 27f + Vector3.up * 0.5f + Vector3.forward * 2f;
        babySpider.StartTrace();

        var highSpider = GameManager.Instance.Monster.MonsterPools[MonsterEnName.HighSpider].Pool.Get();
        highSpider.Initialize();
        highSpider.transform.parent = transform;
        highSpider.transform.position = Vector3.right * 27f + Vector3.up * 0.5f + Vector3.right * 2f;
        highSpider.StartTrace();

        var worm = GameManager.Instance.Monster.MonsterPools[MonsterEnName.Worm].Pool.Get();
        worm.Initialize();
        worm.transform.parent = transform;
        worm.transform.position = Vector3.right * 29f + Vector3.up * 0.5f + Vector3.left * 2f;
        worm.StartTrace();

        var littleWorm = GameManager.Instance.Monster.MonsterPools[MonsterEnName.LittleWorm].Pool.Get();
        littleWorm.Initialize();
        littleWorm.transform.parent = transform;
        littleWorm.transform.position = Vector3.right * 29f + Vector3.up * 0.5f + Vector3.forward * 2f;
        littleWorm.StartTrace();

        var bossWorm = GameManager.Instance.Monster.MonsterPools[MonsterEnName.BossWorm].Pool.Get();
        bossWorm.Initialize();
        bossWorm.transform.parent = transform;
        bossWorm.transform.position = Vector3.right * 29f + Vector3.up * 0.5f + Vector3.right * 2f;
        bossWorm.StartTrace();
    }
}