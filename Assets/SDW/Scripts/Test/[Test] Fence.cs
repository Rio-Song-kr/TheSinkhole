using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFence : MonoBehaviour, IDamageable
{
    [SerializeField] private int m_fenceDurability = 100;

    public void TakenDamage(int damage)
    {
        m_fenceDurability -= damage;

        if (m_fenceDurability <= 0) Destroy(gameObject);
    }
}