using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 설치 관련 데이터들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 관리
/// </summary>
public class ActionManager : MonoBehaviour
{
    //# ActionID, DevelopSO
    private Dictionary<int, DevelopSO> m_actionIdData;

    /// <summary>
    /// Action id별 데이터를 반환
    /// </summary>
    public Dictionary<int, DevelopSO> ActionIdData => m_actionIdData;

    /// <summary>
    /// Action에 따라 Effect ID 전달(EffectID가 없으면 -1)
    /// </summary>
    public Action<int> OnActionEffect;

    private void OnEnable()
    {
        m_actionIdData = new Dictionary<int, DevelopSO>();

        string[] actionConsumedLines = LoadCSV.LoadFromCsv("ActionConsumedItem");
        var actionConsumedList = ReadDataFromLines(actionConsumedLines);

        foreach (var actionConsumed in actionConsumedList)
        {
            var developSO = new DevelopSO();

            if (!m_actionIdData.ContainsKey(actionConsumed.ActionId))
            {
                developSO.RequireItems = new List<RequireItemData>();
            }
            developSO.DevelopDesc = actionConsumed.Description;

            var newRequireItem = new RequireItemData();
            newRequireItem.ItemName = GameManager.Instance.Item.ItemIdEnName[actionConsumed.ItemId];
            newRequireItem.RequireCount = actionConsumed.ItemAmount;

            developSO.RequireItems.Add(newRequireItem);
            m_actionIdData[actionConsumed.ActionId] = developSO;
        }


        //todo ActionConsumedItem에서 한 ID에 필요한 아이템과 수량을 확인하는 방법
        // foreach (var requireItem in GameManager.Instance.Action.ActionIdData[50501].RequireItems)
        // {
        //     Debug.Log($"{requireItem.ItemName}, {requireItem.RequireCount}");
        // }
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 ActionConsumedItemFileData구조체에 맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 ActionConsumedItemFileData</returns>
    private List<ActionConsumedItemFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<ActionConsumedItemFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 4)
            {
                var data = new ActionConsumedItemFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}