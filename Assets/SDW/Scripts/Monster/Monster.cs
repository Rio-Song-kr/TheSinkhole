using UnityEngine;

/// <summary>
/// 몬스터 Id, Health, Speed와 필수로 구현해야하는 메서드를 포함하는 몬스터 추상 클래스
/// </summary>
[System.Serializable]
public abstract class Monster : MonoBehaviour
{
    protected int m_monsterId;
    protected int m_monsterHealth;
    protected int m_monsterSpeed;
    private Transform m_targetTransform;
    private MonsterDataSO m_monsterData;

    private void Awake() => m_monsterData = GameManager.Instance.Monster.MonsterIdDataSO[m_monsterId];

    //! 의사코드
    //# 1. 가까운 울타리 타겟팅
    //# 2. 타겟팅 대상으로 이동
    //# 3. 몬스터 인식범위에 플레이어가 있는가?
    //@ 3.1. 없음 - 울타리 타겟팅 유지
    //@ 3.2. 플레이어가 쉘터 타일 바깥에 있는가?
    //@ 3.2.1 없음 - 울타리 타겟팅 유지
    //@ 3.2.2 있음 - 플레이어 타겟팅

    protected virtual void MoveToTarget()
    {
        //# 접근 
    }

    protected virtual void FindFence()
    {
    }

    protected bool FindPlayer() => false;

    public abstract void Attack();
    public abstract void Defence();
    public abstract void Hit();

    public void SetMonsterId(int id) => m_monsterId = id;
    public int GetMonsterId() => m_monsterId;
    public void SetMonsterHealth(int health) => m_monsterHealth = health;
    public int GetMonsterHealth() => m_monsterHealth;
    public void SetMonsterSpeed(int speed) => m_monsterSpeed = speed;
    public int GetMonsterSpeed() => m_monsterSpeed;
}