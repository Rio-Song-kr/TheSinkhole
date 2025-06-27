using System;

/// <summary>
/// CSV에서 RecipeResult 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct RecipeResultFileData
{
    public int RecipeId;
    public int ItemId;
    public int ItemQuantity;
    public float ItemMakingTime;
    public string Description;

    public RecipeResultFileData(string[] fields)
    {
        RecipeId = int.Parse(fields[0]);
        ItemId = int.Parse(fields[1]);
        ItemQuantity = int.Parse(fields[2]);
        ItemMakingTime = float.Parse(fields[3]);
        Description = fields[4];
    }
}