using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 몬스터 생성을 위한 클래스
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    //! 의사코드
    //# 1. 밤이라면 MonsterManager에서 Monster를 선택해야 함
    //@ 1-1. Random으로 몬스터를 선택(날짜별 확률?)
    //# 2. 몬스터 생성
    //# 3. 날짜 * 3 만큼 생성 되었는가?
    //@ 3-1. 날짜만큼 생성되었으면 종료
    //@ 3-2. 날짜만큼 생성되지 않았으면 계속 생성
    //@ 3-2-1. 단, 직전 몬스터 생성된 후 일정 시간 뒤 생성

    //! 의문 사항 정리
    //# 첫 날은 1마리? 아니면 첫 날도 날짜 * 3?
    //# 각 등급별 몬스터의 생성 확률은 어떻게 할 것인가
    //# 밤 => 낮이 되더라고 재생성이 아닌, 다 잡을 때까지는 유지

    [SerializeField] private float m_creationInvervalTime = 1f;
    private float m_creationTime = 0f;
    private List<int> m_currentQuantity = new List<int>();

    private bool m_isDayInit = true;

    private Dictionary<int, List<int>> m_dayMonsterId;
    public Dictionary<int, List<int>> DayMonsterId => m_dayMonsterId;

    private Dictionary<int, List<int>> m_dayMonsterQuantity;
    public Dictionary<int, List<int>> DayMonsterQuantity => m_dayMonsterQuantity;

    private Dictionary<int, List<int>> m_dayMonsterInitialQuantity;
    public Dictionary<int, List<int>> DayMonsterInitialQuantity => m_dayMonsterInitialQuantity;

    private void OnEnable()
    {
        m_dayMonsterId = new Dictionary<int, List<int>>();
        m_dayMonsterQuantity = new Dictionary<int, List<int>>();
        m_dayMonsterInitialQuantity = new Dictionary<int, List<int>>();

        string[] dayLines = LoadCSV.LoadFromCsv("MonsterRespawn");
        var dayList = ReadDataFromLines(dayLines);

        foreach (var day in dayList)
        {
            if (!m_dayMonsterId.ContainsKey(day.GameDay))
            {
                m_dayMonsterId[day.GameDay] = new List<int>();
                m_dayMonsterQuantity[day.GameDay] = new List<int>();
                m_dayMonsterInitialQuantity[day.GameDay] = new List<int>();
            }

            m_dayMonsterId[day.GameDay].Add(day.MonsterId);
            m_dayMonsterQuantity[day.GameDay].Add(day.MonsterQuantity);
            m_dayMonsterInitialQuantity[day.GameDay].Add(day.MonsterStartQuantity);
        }
    }

    private void Update()
    {
        if (GameTimer.IsDay)
        {
            m_isDayInit = true;
            m_currentQuantity.Clear();
            return;
        }

        if (GameManager.Instance.IsGameOver) return;

        int day = GameTimer.Day == 0 ? 1 : GameTimer.Day;
        Debug.Log(day);

        if (m_isDayInit)
        {
            m_isDayInit = false;
            m_currentQuantity = new List<int>(m_dayMonsterQuantity[day]);

            for (int i = 0; i < m_dayMonsterId[day].Count; i++)
            {
                int initialCount = m_dayMonsterInitialQuantity[day][i];
                for (int j = 0; j < initialCount; j++)
                {
                    //# 해당 날짜의 초기 몬스터 생성
                    var monsterEnName = GameManager.Instance.Monster.MonsterIdDataSO[m_dayMonsterId[day][i]].MonsterEnName;
                    var monster = GameManager.Instance.Monster.MonsterPools[monsterEnName].Pool.Get();
                    int randomPosition = Random.Range(0, GameManager.Instance.Tile.MonsterSwanTile.Count);
                    monster.transform.position = GameManager.Instance.Tile.MonsterSwanTile[randomPosition];
                    monster.Initialize();
                    monster.StartTrace();

                    m_currentQuantity[i]--;
                }
            }
        }

        m_creationTime += Time.deltaTime;

        if (m_creationTime >= m_creationInvervalTime)
        {
            m_creationTime = 0;

            //# 남은 몬스터 수량 확인
            int totalRemaining = m_currentQuantity.Sum();
            if (totalRemaining > 0)
            {
                //# 남은 수량이 있는 몬스터 타입의 인덱스 목록 생성
                var availableMonsters = new List<int>();
                for (int i = 0; i < m_currentQuantity.Count; i++)
                {
                    if (m_currentQuantity[i] > 0)
                    {
                        availableMonsters.Add(i);
                    }
                }

                if (availableMonsters.Count > 0)
                {
                    //# 남은 몬스터 타입 중 랜덤 선택
                    int randomIndex = Random.Range(0, availableMonsters.Count);
                    int monsterIndex = availableMonsters[randomIndex];
                    int monsterId = m_dayMonsterId[day][monsterIndex];

                    var monsterEnName = GameManager.Instance.Monster.MonsterIdDataSO[monsterId].MonsterEnName;
                    var monster = GameManager.Instance.Monster.MonsterPools[monsterEnName].Pool.Get();
                    int randomPosition = Random.Range(0, GameManager.Instance.Tile.MonsterSwanTile.Count);
                    monster.transform.position = GameManager.Instance.Tile.MonsterSwanTile[randomPosition];
                    monster.Initialize();
                    monster.StartTrace();

                    //# 남은 수량 감소
                    m_currentQuantity[monsterIndex]--;
                }
            }
        }
    }

    private List<MonsterDay> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<MonsterDay>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 4)
            {
                var data = new MonsterDay(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}