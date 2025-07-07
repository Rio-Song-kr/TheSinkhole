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
    public StatusType Type;
    public int StatusId;
    public float StatusAmount;
    public string EffectText;

    public EffectFileData(string[] fields)
    {
        EffectId = int.Parse(fields[0]);
        EffectName = fields[1];
        EffectEnName = fields[2];

        if (!Enum.TryParse<StatusType>(fields[3], true, out var statusType))
            statusType = StatusType.None; // 기본값 설정
        Type = statusType;
        StatusId = int.Parse(fields[4]);
        StatusAmount = float.Parse(fields[5]);
        EffectText = fields[6];
    }
}