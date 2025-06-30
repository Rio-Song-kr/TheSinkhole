using System;

/// <summary>
/// CSV에서 Item 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ItemFileData
{
    public int ItemId;
    public string ItemName;
    public string ItemEnName;
    public string ItemType;
    public int ItemMaxOwn;
    public int EffectId;
    public string ItemText;

    public ItemFileData(string[] fields)
    {
        ItemId = int.Parse(fields[0]);
        ItemName = fields[1];
        ItemEnName = fields[2];
        ItemType = fields[3];
        ItemMaxOwn = int.Parse(fields[4]);
        EffectId = int.Parse(fields[5]);
        ItemText = fields[6];
    }
}