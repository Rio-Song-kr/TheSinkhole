using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 몬스터들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 몬스터 풀을 생성하고 관리
/// </summary>
public class MonsterManager : MonoBehaviour
{
    [SerializeField] private MonsterPrefabDatabaseSO m_monsterPrefabDatabaseSO;
    [SerializeField] private GameObject m_sceneMonsterPrefab;

    private Dictionary<MonsterEnName, MonsterPool<SceneMonster>> m_monsterPools;

    /// <summary>
    /// 몬스터별 오브젝트 풀 딕셔너리 반환
    /// </summary>
    public Dictionary<MonsterEnName, MonsterPool<SceneMonster>> MonsterPools => m_monsterPools;

    private Dictionary<MonsterEnName, MonsterDataSO> m_monsterEnDataSO;

    /// <summary>
    /// 몬스터 ID별 MonsterDataSO를 반환
    /// </summary>
    public Dictionary<MonsterEnName, MonsterDataSO> MonsterEnDataSO => m_monsterEnDataSO;

    private Dictionary<int, MonsterDataSO> m_monsterIdDataSO;

    /// <summary>
    /// 몬스터 ID별 MonsterDataSO를 반환
    /// </summary>
    public Dictionary<int, MonsterDataSO> MonsterIdDataSO => m_monsterIdDataSO;

    /// <summary>
    /// CSV 데이터를 로드하여 몬스터 데이터베이스와 오브젝트 풀을 초기화
    /// </summary>
    private void OnEnable()
    {
        if (m_monsterPrefabDatabaseSO.Equals(null))
        {
            Debug.Log("Monster Database SO가 연결되지 않았습니다.");
            return;
        }

        //# 확장자없이 파일 이름 문자열만 사용
        string[] monsterLines = LoadCSV.LoadFromCsv("Monster");
        var monsterList = ReadDataFromLines(monsterLines);

        m_monsterPools = new Dictionary<MonsterEnName, MonsterPool<SceneMonster>>();
        m_monsterEnDataSO = new Dictionary<MonsterEnName, MonsterDataSO>();

        var parentObject = new GameObject();
        parentObject.name = "Monsters";

        foreach (var monster in monsterList)
        {
            //# Scriptable Object 생성
            var newMonsterDataSO = ScriptableObject.CreateInstance<MonsterDataSO>();
            newMonsterDataSO.name = monster.MonsterId.ToString();

            //# string을 위한 enum을 읽어옴
            if (!Enum.TryParse<MonsterTierType>(monster.MonsterTierType, true, out var monsterTierType))
                monsterTierType = MonsterTierType.None;
            if (!Enum.TryParse<MonsterEnName>(monster.MonsterEnName, true, out var monsterEnName))
                monsterEnName = MonsterEnName.None;
            if (!Enum.TryParse<MonsterAttackType>(monster.MonsterAtkType, true, out var monsterAttackType))
                monsterAttackType = MonsterAttackType.None;

            m_monsterPools.Add(monsterEnName, new MonsterPool<SceneMonster>());

            //# CSV에서 읽은 Data 연결
            newMonsterDataSO.MonsterEnName = monsterEnName;

            m_monsterPrefabDatabaseSO.OnSetPrefab(ref newMonsterDataSO);

            //todo 데이터 초기화 및 모델, Icon 등 연결

            var monsterObject = Instantiate(m_sceneMonsterPrefab);
            monsterObject.transform.parent = parentObject.transform;
            monsterObject.tag = "Monster";

            var monsterModel = Instantiate(newMonsterDataSO.ModelPrefab);
            // monsterModel.SetActive(false);
            monsterModel.transform.parent = monsterObject.transform;

            switch (monsterEnName)
            {
                case MonsterEnName.BabySpider:
                case MonsterEnName.Spider:
                case MonsterEnName.HighSpider:
                    newMonsterDataSO.Monster = monsterModel.GetComponent<Spider>();
                    break;
                case MonsterEnName.LittleWorm:
                case MonsterEnName.Worm:
                case MonsterEnName.BossWorm:

                    newMonsterDataSO.Monster = monsterModel.GetComponent<Worm>();
                    break;
                default:
                    Debug.LogError("해당 몬스터 이름은 없습니다.");
                    return;
            }

            newMonsterDataSO.Monster.MonsterId = monster.MonsterId;
            newMonsterDataSO.MonsterName = monster.MonsterName;
            newMonsterDataSO.MonsterTierType = monsterTierType;
            newMonsterDataSO.Monster.MonsterHealth = monster.MonsterHealth;
            newMonsterDataSO.Monster.MonsterSpeed = monster.MonsterSpeed;
            newMonsterDataSO.MaxMonsterHealth = monster.MonsterHealth;
            newMonsterDataSO.MaxMonsterSpeed = monster.MonsterSpeed;
            newMonsterDataSO.MonsterAttackType = monsterAttackType;
            newMonsterDataSO.MonsterAttack = monster.MonsterAttack;
            newMonsterDataSO.MonsterAtkSpeed = monster.MonsterAtkSpeed;
            newMonsterDataSO.MonsterAtkRange = monster.MonsterAtkRange;
            newMonsterDataSO.MonsterDetectDistance = monster.MonsterResearch;
            newMonsterDataSO.MonsterDropItemId = monster.MonsterDropItemId;
            newMonsterDataSO.MonsterDropItemQuantity = monster.MonsterDropItemQuantity;
            newMonsterDataSO.MonsterDescription = monster.MonsterDescription;

            var sceneMonster = monsterObject.GetComponent<SceneMonster>();
            sceneMonster.MonsterDataSO = newMonsterDataSO;

            monsterObject.SetActive(false);


            m_monsterPools[newMonsterDataSO.MonsterEnName].SetPool(monsterObject);
            m_monsterEnDataSO[newMonsterDataSO.MonsterEnName] = newMonsterDataSO;
        }
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 MonsterFileData 구조체에 맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 MonsterFileData</returns>
    private List<MonsterFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<MonsterFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 14)
            {
                var data = new MonsterFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}