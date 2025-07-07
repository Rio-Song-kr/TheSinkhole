using System;

/// <summary>
/// CSV에서 Effect 데이터를 읽어오기 위한 Struct
/// </summary>
[Serializable]
public struct EffectFileData
{
    public int EffectId;
    public string EffectName;
    public string EffectEnName;
    public int StatusId;
    public float StatusAmount;
    public string EffectText;

    public EffectFileData(string[] fields)
    {
        EffectId = int.Parse(fields[0]);
        EffectName = fields[1];
        EffectEnName = fields[2];
        StatusId = int.Parse(fields[3]);
        StatusAmount = float.Parse(fields[4]);
        EffectText = fields[5];
    }
}