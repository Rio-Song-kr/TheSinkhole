using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 쉘터 관련 데이터들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 관리
/// </summary>
public class ShelterManager : MonoBehaviour
{
    //# ShelterLevel, ShelterLevelFileData
    private Dictionary<int, ShelterLevelFileData> m_shelterLevelData;

    /// <summary>
    /// 쉘터 id별 레벨 데이터를 반환
    /// </summary>
    public Dictionary<int, ShelterLevelFileData> ShelterLevelData => m_shelterLevelData;

    //# ShelterLevel, ShelterUpgradeFileData
    private Dictionary<int, List<ShelterUpgradeFileData>> m_shelterUpgradeData;

    /// <summary>
    /// 쉘터 id별 업그레이드 데이터를 반환
    /// </summary>
    public Dictionary<int, List<ShelterUpgradeFileData>> ShelterUpgradeData => m_shelterUpgradeData;

    /// <summary>
    /// Level, LevelID
    /// </summary>
    public Dictionary<int, int> ShelterLevelToId { get; private set; } = new Dictionary<int, int>();

    private void Awake()
    {
        m_shelterLevelData = new Dictionary<int, ShelterLevelFileData>();
        m_shelterUpgradeData = new Dictionary<int, List<ShelterUpgradeFileData>>();

        //# Level Data
        string[] levelLines = LoadCSV.LoadFromCsv("ShelterLevel");
        var levelList = ReadLevelDataFromLines(levelLines);

        foreach (var level in levelList)
        {
            m_shelterLevelData[level.ShId] = level;
            ShelterLevelToId[level.ShLevel] = level.ShId;
        }

        //# Upgrade Data
        string[] upgradeLines = LoadCSV.LoadFromCsv("ShelterUpgrade");
        var upgradeList = ReadUpgradeDataFromLines(upgradeLines);

        foreach (var upgrade in upgradeList)
        {
            if (!m_shelterUpgradeData.ContainsKey(upgrade.ShId))
            {
                m_shelterUpgradeData[upgrade.ShId] = new List<ShelterUpgradeFileData>();
            }
            m_shelterUpgradeData[upgrade.ShId].Add(upgrade);
        }
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 ShelterUpgradeFileData 구조체에 맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 ShelterUpgradeFileData</returns>
    private List<ShelterUpgradeFileData> ReadUpgradeDataFromLines(string[] lines)
    {
        var dataList = new List<ShelterUpgradeFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 4)
            {
                var data = new ShelterUpgradeFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 ShelterLevelFileData 구조체에 맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 ShelterLevelFileData</returns>
    private List<ShelterLevelFileData> ReadLevelDataFromLines(string[] lines)
    {
        var dataList = new List<ShelterLevelFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 4)
            {
                var data = new ShelterLevelFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}