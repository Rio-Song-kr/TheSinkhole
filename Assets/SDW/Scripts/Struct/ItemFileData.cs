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
}