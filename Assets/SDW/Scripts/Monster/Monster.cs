using UnityEngine;

/// <summary>
/// 몬스터 Id, Health, Speed와 필수로 구현해야하는 메서드를 포함하는 몬스터 추상 클래스
/// </summary>
[System.Serializable]
public abstract class Monster : MonoBehaviour
{
    public int MonsterId;
    public int MonsterHealth;
    public int MonsterSpeed;

    public abstract void Attack();
    public abstract void Defence();
    public abstract void Hit();
}