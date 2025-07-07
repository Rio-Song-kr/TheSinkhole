using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임 내 아이템 레시피 데이터들을 관리하는 매니저 클래스
/// CSV 데이터를 로드하여 관리
/// </summary
public class ItemRecipeManager : MonoBehaviour
{
    private Dictionary<int, List<ItemRecipeFileData>> m_recipeIdData;

    /// <summary>
    /// Recipe Id별 데이터
    /// </summary>
    public Dictionary<int, List<ItemRecipeFileData>> RecipeIdData => m_recipeIdData;

    private Dictionary<int, HashSet<int>> m_stationIdRecipeList;

    /// <summary>
    /// Station Id별 데이터
    /// </summary>
    public Dictionary<int, HashSet<int>> StationIdRecipeList => m_stationIdRecipeList;

    private Dictionary<int, int> m_itemIdToRecipeId;

    /// <summary>
    /// 만들어지는 Item Id와 Recipe Id 매칭
    /// </summary>
    public Dictionary<int, int> ItemIdToRecipeId;

    private void Awake()
    {
        m_recipeIdData = new Dictionary<int, List<ItemRecipeFileData>>();
        m_stationIdRecipeList = new Dictionary<int, HashSet<int>>();

        ItemRecipeInit();

        string[] itemRecipeLines = LoadCSV.LoadFromCsv("ItemRecipe");
        var itemRecipeList = ReadDataFromLines(itemRecipeLines);

        foreach (var itemRecipe in itemRecipeList)
        {
            if (!m_stationIdRecipeList.ContainsKey(itemRecipe.StationId))
                m_stationIdRecipeList[itemRecipe.StationId] = new HashSet<int>();

            if (!m_recipeIdData.ContainsKey(itemRecipe.RecipeId))
                m_recipeIdData[itemRecipe.RecipeId] = new List<ItemRecipeFileData>();

            m_stationIdRecipeList[itemRecipe.StationId].Add(itemRecipe.RecipeId);
            m_recipeIdData[itemRecipe.RecipeId].Add(itemRecipe);
        }
    }

    /// <summary>
    /// 만들 아이템의 ID와 고유 레시피 ID를 연결
    /// 현재 수량은 1초, 제작 소요 시간은 5초 고정
    /// </summary>
    private void ItemRecipeInit()
    {
        m_itemIdToRecipeId = new Dictionary<int, int>();
        m_itemIdToRecipeId[20109] = 20701;
        m_itemIdToRecipeId[20110] = 20702;
        m_itemIdToRecipeId[20112] = 20703;
        m_itemIdToRecipeId[20301] = 20704;
        m_itemIdToRecipeId[20114] = 20705;
        m_itemIdToRecipeId[20305] = 20706;
        m_itemIdToRecipeId[20302] = 20707;
    }

    /// <summary>
    /// CSV에서 읽어온 데이터를 ItemRecipeFileData맞게 변환
    /// </summary>
    /// <param name="lines">CSV 파일에서 읽어온 각 줄 데이터</param>
    /// <returns>각 열별로 구분된 ItemRecipeFileData</returns>
    private List<ItemRecipeFileData> ReadDataFromLines(string[] lines)
    {
        var dataList = new List<ItemRecipeFileData>();

        foreach (string line in lines)
        {
            string[] fields = line.Split(',');

            if (fields.Length >= 7)
            {
                var data = new ItemRecipeFileData(fields);

                dataList.Add(data);
            }
        }

        return dataList;
    }
}