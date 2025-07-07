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

    private Dictionary<CraftingStationType, HashSet<int>> m_stationIdRecipeList;

    /// <summary>
    /// Station 타입별 레시피 Id
    /// </summary>
    public Dictionary<CraftingStationType, HashSet<int>> StationIdRecipeList => m_stationIdRecipeList;

    private Dictionary<int, int> m_recipeIdToItemId;

    /// <summary>
    /// 만들어지는 Item Id와 Recipe Id 매칭
    /// </summary>
    public Dictionary<int, int> RecipeIdToItemId => m_recipeIdToItemId;

    private void Awake()
    {
        m_recipeIdData = new Dictionary<int, List<ItemRecipeFileData>>();
        m_stationIdRecipeList = new Dictionary<CraftingStationType, HashSet<int>>();

        ItemRecipeInit();

        string[] itemRecipeLines = LoadCSV.LoadFromCsv("ItemRecipe");
        var itemRecipeList = ReadDataFromLines(itemRecipeLines);

        foreach (var itemRecipe in itemRecipeList)
        {
            var stationType = CraftingStationType.Processing;
            if (itemRecipe.StationId == 20801) stationType = CraftingStationType.Processing;
            else if (itemRecipe.StationId == 20802) stationType = CraftingStationType.Consume;
            if (!m_stationIdRecipeList.ContainsKey(stationType))
                m_stationIdRecipeList[stationType] = new HashSet<int>();

            if (!m_recipeIdData.ContainsKey(itemRecipe.RecipeId))
                m_recipeIdData[itemRecipe.RecipeId] = new List<ItemRecipeFileData>();

            m_stationIdRecipeList[stationType].Add(itemRecipe.RecipeId);
            m_recipeIdData[itemRecipe.RecipeId].Add(itemRecipe);
        }
    }

    /// <summary>
    /// 만들 고유 레시피 ID와 아이템의 ID를 연결
    /// 현재 수량은 1초, 제작 소요 시간은 5초 고정
    /// </summary>
    private void ItemRecipeInit()
    {
        m_recipeIdToItemId = new Dictionary<int, int>();

        m_recipeIdToItemId[20701] = 20109;
        m_recipeIdToItemId[20702] = 20110;
        m_recipeIdToItemId[20703] = 20112;
        m_recipeIdToItemId[20704] = 20301;
        m_recipeIdToItemId[20705] = 20114;
        m_recipeIdToItemId[20706] = 20305;
        m_recipeIdToItemId[20707] = 20302;
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