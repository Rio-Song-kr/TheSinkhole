using System;

/// <summary>
/// CSV에서 GatherPoint 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct GatherPointFileData
{
    public int GatherId;
    public string GatherName;
    public string GatherType;
    public int RequiredTool;
    public int RespawnTime;
    public int ItemId;
    public int MinQuantity;
    public int MaxQuantity;
    public int Description;

    public GatherPointFileData(string[] fields)
    {
        GatherId = int.Parse(fields[0]);
        GatherName = fields[1];
        GatherType = fields[2];
        RequiredTool = int.Parse(fields[3]);
        RespawnTime = int.Parse(fields[4]);
        ItemId = int.Parse(fields[5]);
        MinQuantity = int.Parse(fields[6]);
        MaxQuantity = int.Parse(fields[7]);
        Description = int.Parse(fields[8]);
    }
}