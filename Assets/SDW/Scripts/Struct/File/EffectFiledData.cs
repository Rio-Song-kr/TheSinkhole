using System;

/// <summary>
/// CSV에서 Effect 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct EffectFiledData
{
    public int EffectId;
    public string EffectName;
    public string EffectEnName;
    public int StatusId;
    public float StatusAmount;
    public float EffectTime;
    public string EffectText;

    public EffectFiledData(string[] fields)
    {
        EffectId = int.Parse(fields[0]);
        EffectName = fields[1];
        EffectEnName = fields[2];
        StatusId = int.Parse(fields[3]);
        StatusAmount = float.Parse(fields[4]);
        EffectTime = float.Parse(fields[5]);
        EffectText = fields[6];
    }
}