using System;

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