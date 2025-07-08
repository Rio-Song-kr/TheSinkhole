using System;

/// <summary>
/// CSV에서 TileFarm 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct TileFarmFileData
{
    public int TileId;
    public int CropsId;
    public int PlantDay;
    public bool IsTilled;
    public bool Watered;
    public bool Harvested;
    public string Description;

    public TileFarmFileData(string[] fields)
    {
        TileId = int.Parse(fields[0]);
        CropsId = int.Parse(fields[1]);
        PlantDay = int.Parse(fields[2]);
        IsTilled = bool.Parse(fields[3]);
        Watered = bool.Parse(fields[4]);
        Harvested = bool.Parse(fields[5]);
        Description = fields[6];
    }
}