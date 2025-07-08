using EPOOutline;
using UnityEngine;

/// <summary>
/// 씬에 배치될 아이템을 관리하는 클래스
/// 아이템 데이터, 수량, 픽업 거리 등을 설정하고 콜라이더 크기를 조정
/// </summary>
public class SceneItem : MonoBehaviour
{
    [Header("Item Settings")]
    public ItemEnName ItemEnName;
    public ItemDataSO ItemDataSO;
    public int ItemAmount = 1;

    [Header("Pickup Settings")]
    public float PickUpDistance = 1f;

    private Collider m_itemCollider;
    public Collider ItemCollider => m_itemCollider;

    private Outlinable m_outline;

    private void Start()
    {
        if (ItemDataSO == null && ItemEnName != ItemEnName.None)
        {
            ItemDataSO = GameManager.Instance.Item.ItemEnDataSO[ItemEnName];
        }

        m_outline = GetComponentInChildren<Outlinable>();
        m_outline.enabled = false;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameOver) return;

        ReturnToPool();
    }

    /// <summary>
    /// 픽업 거리에 맞춰 콜라이더 크기를 설정
    /// SphereCollider의 경우 radius를, BoxCollider의 경우 size를 조정
    /// </summary>
    public void SetupColliderSize()
    {
        m_itemCollider = transform.GetComponent<Collider>();
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

    private void ReturnToPool()
    {
        transform.position = Vector3.down * 50;
        GameManager.Instance.Item.ItemPools[ItemDataSO.ItemEnName].Pool.Release(this);
    }

    public void SetOutline(bool isEnable) => m_outline.enabled = isEnable;
}