using System;

/// <summary>
/// CSV에서 ItemRecipe 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ItemRecipeFileData
{
    public int RecipeId;
    public int MaterialId;
    public string CraftingItemType;
    public string CraftingItemTypeEn;
    public int MaterialQuantity;
    public int StationId;
    public string Description;

    public ItemRecipeFileData(string[] fields)
    {
        RecipeId = int.Parse(fields[0]);
        MaterialId = int.Parse(fields[1]);
        CraftingItemType = fields[2];
        CraftingItemTypeEn = fields[3];
        MaterialQuantity = int.Parse(fields[4]);
        StationId = int.Parse(fields[5]);
        Description = fields[6];
    }
}