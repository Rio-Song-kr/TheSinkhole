using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Camera cam;
    [Header("Attacking")]
    [SerializeField] private ViewmodelManager viewmodelManager;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask attackLayer;

    [Header("Sound")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip weaponSwing;
    [SerializeField] private AudioClip hitSound;
    //private AudioSource audioSource;

    private bool attacking = false;
    private bool readyToAttack = true;

    private void Start()
    {
        attackDamage = PlayerStatus.Instance.AttackPower;
        attackSpeed = PlayerStatus.Instance.AtkSpeed;
    }

    public void Attack()
    {
        if (viewmodelManager.isAttakable == false) return;
        if (!readyToAttack) return;

        Debug.Log("공격 호출");
        readyToAttack = false;
        StartCoroutine(PerformAttack());
        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(weaponSwing);
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(attackDelay);

        AttackRaycast();

        yield return new WaitForSeconds(attackSpeed - attackDelay);

        readyToAttack = true;
    }

    private void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTargetEffect(hit.point);
            Debug.Log("몬스터 공격");
            if (hit.transform.TryGetComponent<Monster>(out Monster T))
            {
                T.TakenDamage(attackDamage);
            }
        }
        else
        {
            Debug.Log("공중에 휘두르기!");
        }
    }

    private void HitTargetEffect(Vector3 pos)
    {
        //audioSource.pitch = 1;
        //audioSource.PlayOneShot(hitSound);
        //GameObject gameObject = Instantiate(hitEffect, pos, Quaternion.identity);
        //Destroy(gameObject, 20);
    }
}
