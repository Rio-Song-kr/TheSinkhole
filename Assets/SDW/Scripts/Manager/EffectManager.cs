using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 효과 관련 데이터들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 관리
/// </summary>
public class EffectManager : MonoBehaviour
{
    //# EffectId, EffectFileData
    private Dictionary<int, EffectFileData> m_effectIdData;

    /// <summary>
    /// Effect id별 데이터를 반환
    /// </summary>
    public Dictionary<int, EffectFileData> EffectIdData => m_effectIdData;

    private void Awake()
    {
        m_effectIdData = new Dictionary<int, EffectFileData>();

        string[] effectLines = LoadCSV.LoadFromCsv("Effect");
        var effectList = ReadDataFromLines(effectLines);

        foreach (var effect in effectList)
        {
            m_effectIdData[effect.EffectId] = effect;
        }
    }

    private List<EffectFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<EffectFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 6)
            {
                var data = new EffectFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}