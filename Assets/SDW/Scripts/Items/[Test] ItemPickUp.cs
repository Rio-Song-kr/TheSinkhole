using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TestItemPickUp : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemDataSO ItemData;
    public int ItemAmount = 1;

    [Header("Pickup Settings")]
    public float PickUpDistance = 1f;
    public LayerMask PlayerLayer;

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;

        SetupColliderSize();
    }

    private void SetupColliderSize()
    {
        switch (_collider)
        {
            case SphereCollider sphere:
                sphere.radius = PickUpDistance;
                break;
            case BoxCollider box:
                box.size = Vector3.one * PickUpDistance;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & PlayerLayer) == 0) return;

        var inventory = other.transform.GetComponent<Inventory>();
        if (!inventory) return;

        int remainingAmount = inventory.InventorySystem.AddItem(ItemData, ItemAmount);
        if (remainingAmount == 0) Destroy(gameObject);
        else ItemAmount = remainingAmount;
    }
}