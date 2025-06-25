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

    /// <summary>
    /// 플레이어와 충돌 시 아이템을 스마트하게 인벤토리에 추가
    /// 마인크래프트 스타일로 기존 스택을 우선 채우고 최적 분배
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    private void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & PlayerLayer) == 0) return;

        var inventory = other.transform.GetComponent<Inventory>();
        if (!inventory) return;

        //# 스마트 추가 시도
        int remainingAmount = inventory.AddItemSmart(ItemData, ItemAmount);

        //@ 모든 아이템이 성공적으로 추가됨
        if (remainingAmount == 0) Destroy(gameObject);
        else if (remainingAmount < ItemAmount)
        {
            //@ 일부만 추가됨 - 남은 수량으로 업데이트
            ItemAmount = remainingAmount;
            Debug.Log($"인벤토리 일부 가득참! 남은 아이템: {remainingAmount}");

            //todo 아이템이 부분적으로 추가되었음을 시각적으로 표시
            //@ 예: 이펙트 재생, 사운드 등
        }
        else
        {
            Debug.Log("인벤토리가 완전히 가득참!");

            //todo 인벤토리가 가득 찼다는 UI 메시지 표시
        }
    }
}