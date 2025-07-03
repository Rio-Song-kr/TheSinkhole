using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;
public class ViewmodelAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CharacterController controller;

    private Vector3 velocity;

    private void Awake()
    {
        controller = GetComponentInParent<CharacterController>();
    }

    private void FixedUpdate()
    {
        GetAnimationByVelocity();
    }

    // 이동속도에 따라 파라미터 수정
    // 
    private void GetAnimationByVelocity()
    {
        velocity = controller.velocity;
        if (velocity.magnitude < 0.1f)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isSprinting", false);
        }
        else if (velocity.magnitude < 3.5f)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", false);
        }
        else if (velocity.magnitude > 3.5f)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isSprinting", true);
        }
    }
    public void WalkStart()
    {
        animator.SetBool("isWalking", true);
    }


    public void WalkStop()
    {
        animator.SetBool("isWalking", false);
    }

    public void SprintStart()
    {
        //animator.get
        animator.SetBool("isSprinting", true);
    }
    public void SprintStop()
    {
        animator.SetBool("isSprinting", false);
    }
}
