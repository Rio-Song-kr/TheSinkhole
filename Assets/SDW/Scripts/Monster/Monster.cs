using System;
using UnityEngine;

/// <summary>
/// 몬스터 Id, Health, Speed와 필수로 구현해야하는 메서드를 포함하는 몬스터 추상 클래스
/// </summary>
[Serializable]
public class Monster : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int MonsterId;
    [field: SerializeField] public int MonsterHealth;
    [field: SerializeField] public int MonsterSpeed;
    public bool IsAlive { get; private set; } = false;
    public Action OnAttack;
    public Action OnDie;
    public Action OnTakenDamaged;

    //! 의사코드
    //# 1. 가까운 울타리 타겟팅
    //# 2. 타겟팅 대상으로 이동
    //# 3. 몬스터 인식범위에 플레이어가 있는가?
    //@ 3.1. 없음 - 울타리 타겟팅 유지
    //@ 3.2. 플레이어가 쉘터 타일 바깥에 있는가?
    //@ 3.2.1 없음 - 울타리 타겟팅 유지
    //@ 3.2.2 있음 - 플레이어 타겟팅

    private void OnEnable() => IsAlive = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            TakenDamage(50);
    }

    public void Attack()
    {
        if (MonsterHealth <= 0) return;
        OnAttack?.Invoke();
    }

    public void Die()
    {
        if (MonsterHealth <= 0) return;
        OnDie?.Invoke();
    }

    public void TakenDamage(int damage)
    {
        if (MonsterHealth <= 0) return;
        OnTakenDamaged?.Invoke();
        MonsterHealth -= damage;

        if (MonsterHealth <= 0)
        {
            MonsterHealth = 0;
            IsAlive = false;
            OnDie?.Invoke();
        }
    }
}