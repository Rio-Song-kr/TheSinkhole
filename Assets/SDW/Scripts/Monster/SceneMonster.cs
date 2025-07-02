using UnityEngine;

/// <summary>
/// 씬에 배치될 몬스터를 관리하는 클래스
/// </summary>
public class SceneMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    public MonsterDataSO MonsterDataSO;
    private Vector3 m_targetPosition;
    private Transform m_playerTransform;

    //# 추적을 위한 필드
    private GameObject m_fence;

    private void Start() => m_playerTransform = GameObject.FindWithTag("Player").transform;

    private void Update()
    {
        if (!MonsterDataSO.Monster.IsAlive)
        {
            GameManager.Instance.Monster.MonsterPools[MonsterDataSO.MonsterEnName].Pool.Release(this);
            return;
        }

        // m_targetPosition = FindTarget();

        //todo navmesh에서 target을 추적해야 함
    }

    public void Initialize()
    {
        //todo 추후 Fence Tag가 추가되면 주석 제거
        m_fence = GameObject.FindWithTag("Fence");

        if (m_fence == null)
        {
            Debug.LogError("Fence가 없습니다.");
            return;
        }

        MonsterDataSO.Monster.IsAlive = true;
        MonsterDataSO.Monster.MonsterHealth = MonsterDataSO.MaxMonsterHealth;
        MonsterDataSO.Monster.MonsterSpeed = MonsterDataSO.MaxMonsterSpeed;
    }

    /// <summary>
    /// Target을 찾기 위한 메서드
    /// 플레이어가 Fence 내에 있으면 Fence의 Position을 반환
    /// 플레이어가 Fence 밖에 있지만, 감지거리 밖에 있으면 Fence의 Position을 반환
    /// 플레이어가 감지거리 내에 있으면 Player의 Position을 반환
    /// </summary>
    /// <returns></returns>
    private Vector3 FindTarget()
    {
        //# 플레이어가 Fence에 있으면 Fence를 향해 걸어감
        if (IsPlayerStayInFence())
        {
            return m_fence.transform.position;
        }

        //# 플레이어가 Fence 밖이면서 감지거리 밖에 있으면 Fence를 향해 걸어감
        if (!FindPlayer()) return m_fence.transform.position;

        return m_playerTransform.position;
    }

    /// <summary>
    /// 몬스터와 플레이어의 거리를 계산하여 감지거리 내에 플레이어가 있는지 확인
    /// </summary>
    /// <returns>감지 거리 내라면 true, 아니면 false를 반환</returns>
    private bool FindPlayer() =>
        Vector3.Distance(transform.position, m_playerTransform.position) <= MonsterDataSO.MonsterDetectDistance;

    /// <summary>
    /// Player가 Fence 내에 있는지 확인하는 메서드
    /// </summary>
    /// <returns>Fence 내에 있으면 true, Fence 내에 없으면 false를 반환</returns>
    private bool IsPlayerStayInFence()
    {
        var fenceXArea = GameManager.Instance.Tile.GetFenceXArea();
        var fenceYArea = GameManager.Instance.Tile.GetFenceYArea();

        if (m_playerTransform.position.x < fenceXArea.x || m_playerTransform.position.x > fenceXArea.y) return false;
        if (m_playerTransform.position.y < fenceYArea.x || m_playerTransform.position.y > fenceYArea.y) return false;

        return true;
    }
}