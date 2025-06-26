using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SceneItem : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemDataSO ItemData;
    public int ItemAmount = 1;

    [Header("Pickup Settings")]
    public float PickUpDistance = 1f;

    private Collider m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider>();

        SetupColliderSize();
    }

    private void SetupColliderSize()
    {
        switch (m_collider)
        {
            case SphereCollider sphere:
                sphere.radius = PickUpDistance;
                break;
            case BoxCollider box:
                box.size = Vector3.one * PickUpDistance;
                break;
        }
    }
}