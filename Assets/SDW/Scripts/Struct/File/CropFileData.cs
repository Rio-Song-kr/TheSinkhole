using System;

/// <summary>
/// CSV에서 Crop 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct CropFileData
{
    public int CropId;
    public string CropName;
    public int ItemId;
    public string Description;

    public CropFileData(string[] fields)
    {
        CropId = int.Parse(fields[0]);
        CropName = fields[1];
        ItemId = int.Parse(fields[2]);
        Description = fields[3];
    }
}