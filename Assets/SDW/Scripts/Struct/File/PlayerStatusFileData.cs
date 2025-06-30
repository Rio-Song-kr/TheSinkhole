using System;

/// <summary>
/// CSV에서 PlayerStatus 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct PlayerStatusFileData
{
    public int StatId;
    public float MaxHP;
    public float MaxMentality;
    public float MaxHungry;
    public float MaxThirst;
    public float PlSpeed;
    public float PlAtkSpeed;
    public string Description;

    public PlayerStatusFileData(string[] fields)
    {
        StatId = int.Parse(fields[0]);
        MaxHP = float.Parse(fields[1]);
        MaxMentality = float.Parse(fields[2]);
        MaxHungry = float.Parse(fields[3]);
        MaxThirst = float.Parse(fields[4]);
        PlSpeed = float.Parse(fields[5]);
        PlAtkSpeed = float.Parse(fields[6]);
        Description = fields[7];
    }
}