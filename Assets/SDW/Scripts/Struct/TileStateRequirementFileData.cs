using System;

/// <summary>
/// CSV에서 TileStateRequirement 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct TileStateRequirementFileData
{
    public string TileType;
    public int RequiredItemId;
    public int RequiredItemQuantity;
    public string Description;

    public TileStateRequirementFileData(string[] fields)
    {
        TileType = fields[0];
        RequiredItemId = int.Parse(fields[1]);
        RequiredItemQuantity = int.Parse(fields[2]);
        Description = fields[3];
    }
}