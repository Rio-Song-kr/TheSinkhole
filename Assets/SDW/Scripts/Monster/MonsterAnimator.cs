using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    private Animator m_animator;
    private int m_hashIsWalk = Animator.StringToHash("IsWalk");
    private int m_hashIsAttack = Animator.StringToHash("IsAttack");
    private int m_hashTakeDamage = Animator.StringToHash("TakeDamage");
    private int m_hashDie = Animator.StringToHash("Die");

    private void Awake() => m_animator = GetComponentInChildren<Animator>();

    public void SetWalk(bool isWalk) => m_animator.SetBool(m_hashIsWalk, isWalk);

    public void SetAttack(bool isAttack) => m_animator.SetBool(m_hashIsAttack, isAttack);

    public void TriggerTakenDamage() => m_animator.SetTrigger(m_hashTakeDamage);

    public void TriggerToDie() => m_animator.SetTrigger(m_hashDie);

    public AnimatorStateInfo GetState() => m_animator.GetCurrentAnimatorStateInfo(0);
}