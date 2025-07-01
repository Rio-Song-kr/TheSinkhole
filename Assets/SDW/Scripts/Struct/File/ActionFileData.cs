using System;

/// <summary>
/// CSV에서 Action 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct ActionFileData
{
    public int ActionId;
    public string ActionName;
    public string ActionEnName;
    public string ActionType;
    public int ItemId;
    public float ActionRange;
    public float Duration;
    public int EffectId;
    public string Description;

    public ActionFileData(string[] fields)
    {
        ActionId = int.Parse(fields[0]);
        ActionName = fields[1];
        ActionEnName = fields[2];
        ActionType = fields[3];
        ItemId = int.Parse(fields[4]);
        ActionRange = float.Parse(fields[5]);
        Duration = float.Parse(fields[6]);
        EffectId = int.Parse(fields[7]);
        Description = fields[8];
    }
}