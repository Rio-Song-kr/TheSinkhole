using System;

/// <summary>
/// Player 정보를 저장/불러오기를 위한 구조체(프로토타입)
/// </summary>
[Serializable]
public struct PlayerSaveData
{
    public float CurHealth;
    public float CurHunger;
    public float CurThirst;
    public float CurMentality;
    public float CurPlayerMoveSpeed;
    public bool isStarving;
    public bool isDehydrated;
}