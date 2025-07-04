using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFence : MonoBehaviour, IDamageable
{
    private int m_fenceDurability = 100;

    public void TakenDamage(int damage)
    {
        m_fenceDurability -= damage;
        Debug.Log(m_fenceDurability);

        if (m_fenceDurability <= 0) Destroy(gameObject);
    }
}