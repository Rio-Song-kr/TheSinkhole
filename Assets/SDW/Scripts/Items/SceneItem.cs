using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemDataSO ItemDataSO;
    public int ItemAmount = 1;

    [Header("Pickup Settings")]
    public float PickUpDistance = 1f;

    private Collider m_itemCollider;
    public Collider ItemCollider => m_itemCollider;

    public void SetupColliderSize()
    {
        m_itemCollider = transform.parent.GetComponent<Collider>();
        switch (m_itemCollider)
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