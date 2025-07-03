using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class ViewmodelAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

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
        Debug.Log("Sprint start");
        animator.SetBool("isSprinting", true);
    }
    public void SprintStop()
    {
        Debug.Log("Sprint End");
        animator.SetBool("isSprinting", false);
    }
}
