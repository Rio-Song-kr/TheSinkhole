using System;

/// <summary>
/// CSV에서 Monster 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct MonsterFileData
{
    public int MonsterId;
    public string MonsterName;
    public string MonsterEnName;
    public string MonsterTierType;
    public int MonsterHealth;
    public int MonsterSpeed;
    public string MonsterAtkType;
    public int MonsterAttack;
    public float MonsterAtkSpeed;
    public float MonsterAtkRange;
    public float MonsterResearch;
    public int MonsterDropItemId;
    public int MonsterDropItemQuantity;
    public string MonsterDescription;

    public MonsterFileData(string[] field)
    {
        MonsterId = int.Parse(field[0]);
        MonsterName = field[1];
        MonsterEnName = field[2];
        MonsterTierType = field[3];
        MonsterHealth = int.Parse(field[4]);
        MonsterSpeed = int.Parse(field[5]);
        MonsterAtkType = field[6];
        MonsterAttack = int.Parse(field[7]);
        MonsterAtkSpeed = float.Parse(field[8]);
        MonsterAtkRange = float.Parse(field[9]);
        MonsterResearch = float.Parse(field[10]);
        MonsterDropItemId = int.Parse(field[11]);
        MonsterDropItemQuantity = int.Parse(field[12]);
        MonsterDescription = field[13];
    }
}