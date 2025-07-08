using System;

/// <summary>
/// CSV에서 ShelterLevel 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ShelterLevelFileData
{
    public int ShId;
    public int ShLevel;
    public int ShelterDurability;
    public string ShelterName;

    public ShelterLevelFileData(string[] fields)
    {
        ShId = int.Parse(fields[0]);
        ShLevel = int.Parse(fields[1]);
        ShelterDurability = int.Parse(fields[2]);
        ShelterName = fields[3];
    }
}