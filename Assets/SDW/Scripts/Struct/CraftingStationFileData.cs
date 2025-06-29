using System;

/// <summary>
/// CSV에서 CraftingStation 데이터를 읽어오기 위한 Struct
/// </summary>
/// 
[Serializable]
public struct CraftingStationFileData
{
    public int StationId;
    public string Stationname;
    public string StationEnName;
    public float InteractRange;
    public string Description;

    public CraftingStationFileData(string[] fields)
    {
        StationId = int.Parse(fields[0]);
        Stationname = fields[1];
        StationEnName = fields[2];
        InteractRange = float.Parse(fields[3]);
        Description = fields[4];
    }
}