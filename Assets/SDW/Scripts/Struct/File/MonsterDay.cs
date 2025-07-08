/// <summary>
/// CSV에서 Monster Day 데이터를 읽어오기 위한 Struct
/// </summary>
public struct MonsterDay
{
    public int GameDay;
    public int MonsterId;
    public int MonsterQuantity;
    public int MonsterStartQuantity;

    public MonsterDay(string[] fields)
    {
        GameDay = int.Parse(fields[0]);
        MonsterId = int.Parse(fields[1]);
        MonsterQuantity = int.Parse(fields[2]);
        MonsterStartQuantity = int.Parse(fields[3]);
    }
}