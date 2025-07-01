using UnityEngine;

[System.Serializable]
public abstract class Monster : MonoBehaviour
{
    public int MonsterId;
    public int MonsterHP;
    public int MonsterSpeed;

    public abstract void Attack();
    public abstract void Defence();
    public abstract void Hit();
}