using System;

/// <summary>
/// CSV에서 ShelterUpgrade 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ShelterUpgradeFileData
{
    public int ShId;
    public int ShUpgradeId;
    public int ItemId;
    public int MaterialQuantity;

    public ShelterUpgradeFileData(string[] fields)
    {
        ShId = int.Parse(fields[0]);
        ShUpgradeId = int.Parse(fields[1]);
        ItemId = int.Parse(fields[2]);
        MaterialQuantity = int.Parse(fields[3]);
    }
}