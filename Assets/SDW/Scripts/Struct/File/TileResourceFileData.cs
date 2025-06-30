using System;

/// <summary>
/// CSV에서 TileResource 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct TileResourceFileData
{
    public int TileId;
    public int ItemId;
    public bool ResourceAvailable;
    public int LastCollectDay;
    public int RespawnCycle;
    public int ResourceAmount;
    public string Description;

    public TileResourceFileData(string[] fields)
    {
        TileId = int.Parse(fields[0]);
        ItemId = int.Parse(fields[1]);
        ResourceAvailable = bool.Parse(fields[2]);
        LastCollectDay = int.Parse(fields[3]);
        RespawnCycle = int.Parse(fields[4]);
        ResourceAmount = int.Parse(fields[5]);
        Description = fields[6];
    }
}