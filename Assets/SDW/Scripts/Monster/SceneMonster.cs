using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 씬에 배치될 몬스터를 관리하는 클래스
/// </summary>
public class SceneMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    public MonsterDataSO MonsterDataSO;
    private Transform m_targetTransform;
    private Transform m_prevTargetTransform;
    private Transform m_playerTransform;

    [Header("Layer To Track")]
    [SerializeField] private LayerMask m_playerMask;
    [SerializeField] private LayerMask m_fenceMask;

    private NavMeshAgent m_navMeshAgent;
    private static WaitForSeconds WaitTime = new WaitForSeconds(0.25f);

    private Collider m_targetCollider;

    //# 추적을 위한 필드
    private GameObject m_fence;

    private void Awake() => m_navMeshAgent = GetComponent<NavMeshAgent>();

    private void Start()
    {
        m_playerTransform = GameObject.FindWithTag("Player").transform;

        m_targetTransform = m_fence.gameObject.transform;
        m_targetCollider = m_targetTransform.GetComponent<Collider>();

        // m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public void StartTrace()
    {
        m_navMeshAgent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out var hit, 3f, NavMesh.AllAreas))
        {
            m_navMeshAgent.Warp(hit.position);
            m_navMeshAgent.Resume();
            StartCoroutine(UpdatePath());
            StartCoroutine(TraverseOffMeshLink());
        }
        else
        {
            Debug.LogError("이동할 수 있는 NavMesh 위치를 찾을 수 없습니다");
        }
    }

    private void Update()
    {
        if (!MonsterDataSO.Monster.IsAlive)
        {
            GameManager.Instance.Monster.MonsterPools[MonsterDataSO.MonsterEnName].Pool.Release(this);
            return;
        }

        m_targetTransform = FindTarget();

        m_targetCollider = m_targetTransform?.GetComponent<Collider>();
        if (m_targetCollider != null)
        {
            // 몬스터와 타겟 경계 간의 최소 거리 계산
            var closestPoint = m_targetCollider.ClosestPoint(transform.position);
            float distanceToBoundary = Vector3.Distance(transform.position, closestPoint);

            if (distanceToBoundary <= m_navMeshAgent.stoppingDistance)
            {
                var lookDirection = (m_targetTransform.position - transform.position).normalized;

                if (lookDirection != Vector3.zero)
                {
                    lookDirection.y = 0;
                    var lookRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }

                m_navMeshAgent.isStopped = true;
                m_navMeshAgent.velocity = Vector3.zero;
            }
            else
            {
                m_navMeshAgent.isStopped = false;
            }
        }
    }

    public void Initialize()
    {
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
    private Transform FindTarget()
    {
        Transform targetTransform;
        //# 플레이어가 Fence에 있으면 Fence를 향해 걸어감
        if (IsPlayerStayInFence())
        {
            if (m_fence == null)
            {
                Debug.LogWarning($"{MonsterDataSO.MonsterEnName} - m_fence null");
            }
            targetTransform = m_fence.transform;
            return targetTransform;
        }

        //# 플레이어가 Fence 밖이면서 감지거리 밖에 있으면 Fence를 향해 걸어감
        if (!FindPlayer())
        {
            targetTransform = m_fence.transform;
            return targetTransform;
        }

        targetTransform = m_playerTransform;

        return targetTransform;
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

    private IEnumerator UpdatePath()
    {
        //todo Game Over가 아니라면 계속 반복
        while (!GameManager.Instance.IsGameOver)
        {
            if (!m_navMeshAgent.pathPending && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
            {
                m_navMeshAgent.isStopped = true;
            }
            else
                m_navMeshAgent.isStopped = false;
            yield return WaitTime;
            m_navMeshAgent.SetDestination(m_targetTransform.position);
            yield return WaitTime;
        }
    }

    /// <summary>
    /// OffLink를 지날 때 속도가 증가되는 문제를 해결하기 위한 코루틴
    /// 수동으로 OffLink를 지날 때 로직 처리
    /// </summary>
    private IEnumerator TraverseOffMeshLink()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            if (m_navMeshAgent.isOnOffMeshLink)
            {
                var linkData = m_navMeshAgent.currentOffMeshLinkData;
                var startPos = m_navMeshAgent.transform.position;
                var endPos = linkData.endPos;

                float duration = Vector3.Distance(startPos, endPos) / m_navMeshAgent.speed;
                float t = 0;

                while (t < duration)
                {
                    m_navMeshAgent.transform.position = Vector3.Lerp(startPos, endPos, t / duration);
                    t += Time.deltaTime;
                    yield return null;
                }

                m_navMeshAgent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }
}