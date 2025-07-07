using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
public class ViewmodelAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CharacterController controller;

    private float delayTimer = 0.5f;
    private bool allowAnimation;
    private Vector3 velocity;

    private bool canAttack = true;
    [SerializeField] float attackCooldown = 1f;

    private void Awake()
    {
        controller = GetComponentInParent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (allowAnimation)
        {
            GetAnimationByVelocity();
        }
        else
        {
            delayTimer -= Time.fixedDeltaTime;
            if (delayTimer <= 0f) allowAnimation = true;
        }
    }

    // 이동속도에 따라 파라미터 수정
    // 
    private void GetAnimationByVelocity()
    {
        velocity = controller.velocity;
        velocity.y = 0f;
        if (GameManager.Instance.IsCursorLocked == false)
        {
            SetIdle();
        }
        if (velocity.magnitude < 0.1f)
        {
            SetIdle();
        }
        else if (velocity.magnitude < 3.5f && GameManager.Instance.IsCursorLocked == true)
        {
            SetWalking();

        }
        else if (velocity.magnitude > 3.5f && GameManager.Instance.IsCursorLocked == true)
        {
            SetSprinting();
        }
    }

    public void SetAttack()
    {
        if (!canAttack) return;
        animator.SetTrigger("Attack");
        canAttack = false;
        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // 추후에 사운드 or 이펙트를 추가할 경우를 대비해 각각 상태에 대해 별도의 함수화.
    private void SetIdle()
    {
        Debug.Log("대기 상태");
        animator.SetBool("isWalking", false);
        animator.SetBool("isSprinting", false);
    }
    private void SetWalking()
    {
        Debug.Log("걷기 상태");
        animator.SetBool("isWalking", true);
        animator.SetBool("isSprinting", false);
    }
    private void SetSprinting()
    {
        Debug.Log("달리기 상태");
        animator.SetBool("isWalking", true);
        animator.SetBool("isSprinting", true);
    }
}
