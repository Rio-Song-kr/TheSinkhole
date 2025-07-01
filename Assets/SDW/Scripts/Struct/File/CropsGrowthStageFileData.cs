using System;

/// <summary>
/// CSV에서 CropsGrowthStage 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct CropsGrowthStageFileData
{
    public int CropStageId;
    public int CropId;
    public int Stage;
    public int DayRequired;
    public string Description;

    public CropsGrowthStageFileData(string[] fields)
    {
        CropStageId = int.Parse(fields[0]);
        CropId = int.Parse(fields[1]);
        Stage = int.Parse(fields[2]);
        DayRequired = int.Parse(fields[3]);
        Description = fields[4];
    }
}