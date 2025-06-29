using System;

/// <summary>
/// CSV에서 ActionConsumedItem 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ActionConsumedItemFiledData
{
    public int ActionId;
    public int ItemId;
    public int ItemAmount;
    public string Description;

    public ActionConsumedItemFiledData(string[] fields)
    {
        ActionId = int.Parse(fields[0]);
        ItemId = int.Parse(fields[1]);
        ItemAmount = int.Parse(fields[2]);
        Description = fields[3];
    }
}