using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 씬에 배치될 몬스터를 관리하는 클래스
/// </summary>
public class SceneMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    private Monster m_monster;
    private NavMeshAgent m_navMeshAgent;
    private MonsterAnimator m_monsterAnimator;
    public MonsterDataSO MonsterDataSO;

    [Header("Layer To Track")]
    [SerializeField] private LayerMask m_targetMask;

    private Transform m_targetTransform;
    private Transform m_prevTargetTransform;
    private Transform m_playerTransform;
    private Transform m_fenceTransform;

    private MonsterState m_monsterState = MonsterState.Walk;

    private static WaitForSeconds WaitTime = new WaitForSeconds(0.25f);

    private Collider m_targetCollider;
    private RaycastHit Hit;

    private void Awake()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_monsterAnimator = GetComponent<MonsterAnimator>();
    }

    private void OnDisable()
    {
        if (m_monster == null) return;
        m_monster.OnAttack -= OnAttack;
        m_monster.OnDie -= OnDie;
        m_monster.OnTakenDamaged -= OnTakenDamaged;
    }

    private void Start()
    {
        m_targetTransform = m_fenceTransform;
        m_targetCollider = m_targetTransform.GetComponent<Collider>();
    }

    private void Update()
    {
        if (!m_monster.IsAlive)
        {
            m_monsterAnimator.SetAttack(false);
            m_monsterAnimator.SetWalk(false);

            var stateInfo = m_monsterAnimator.GetState();
            if (stateInfo.IsName("Exit") || stateInfo.IsName("Die") && stateInfo.normalizedTime >= 1.0f)
            {
                var itemEnName = GameManager.Instance.Item.ItemIdEnName[MonsterDataSO.MonsterDropItemId];
                var item = GameManager.Instance.Item.ItemPools[itemEnName].Pool.Get();
                item.ItemAmount = MonsterDataSO.MonsterDropItemQuantity;
                item.transform.position = transform.position;

                ReturnToPool();
            }
            return;
        }

        if (GameManager.Instance.IsGameOver)
        {
            ReturnToPool();

            return;
        }

        m_targetTransform = FindTarget();

        if (m_targetTransform == null) return;

        bool isTargetClose = IsTargetClose();

        switch (m_monsterState)
        {
            case MonsterState.Walk:
                HandleMoveAndStop(isTargetClose);
                break;
            case MonsterState.Attack:
                HandleAttack(isTargetClose);
                break;
        }
    }

    private void ReturnToPool()
    {
        transform.position = Vector3.down * 50;
        m_navMeshAgent.enabled = false;
        GameManager.Instance.Monster.MonsterPools[MonsterDataSO.MonsterEnName].Pool.Release(this);
    }

    /// <summary>
    /// 초기에는 Fence를 찾아서 목적지로 설정
    /// </summary>
    public void Initialize()
    {
        if (m_monster == null)
            m_monster = GetComponentInChildren<Monster>();
        m_fenceTransform = GameObject.FindWithTag("Fence")?.transform;

        if (m_fenceTransform == null)
        {
            Debug.LogError("Fence가 없습니다.");
            return;
        }

        m_playerTransform = GameObject.FindWithTag("Player").transform;

        if (m_playerTransform == null)
        {
            Debug.LogError("Player가 없습니다.");
            return;
        }

        m_monster.MonsterHealth = MonsterDataSO.MaxMonsterHealth;
        m_monster.MonsterSpeed = MonsterDataSO.MaxMonsterSpeed;

        m_monsterState = MonsterState.Walk;

        m_monster.OnAttack += OnAttack;
        m_monster.OnDie += OnDie;
        m_monster.OnTakenDamaged += OnTakenDamaged;
    }

    /// <summary>
    /// MonsterPool에서 꺼내면서 추적을 위한 초기 세팅을 설정
    /// </summary>
    public void StartTrace()
    {
        m_navMeshAgent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out var hit, 3f, NavMesh.AllAreas))
        {
            m_navMeshAgent.Warp(hit.position);
            m_navMeshAgent.stoppingDistance = 3f;
            // m_navMeshAgent.Resume();
            StartCoroutine(UpdatePath());

            //# MeshLink에서 사용했던 코드 - 레거시
            // StartCoroutine(TraverseOffMeshLink());
        }
        else
        {
            Debug.LogError("이동할 수 있는 NavMesh 위치를 찾을 수 없습니다");
        }
    }

    /// <summary>
    /// 몬스터가 타겟 주위에 있는지 확인
    /// </summary>
    /// <returns>몬스터 주위에 있다면 true, 아니면 false를 반환</returns>
    private bool IsTargetClose()
    {
        // 몬스터와 타겟 경계 간의 최소 거리 계산
        if (m_targetTransform == null || m_targetCollider == null) return false;

        var closestPoint = m_targetCollider.ClosestPoint(transform.position);

        float distanceToBoundary = Vector3.Distance(transform.position, closestPoint);

        return distanceToBoundary <= m_navMeshAgent.stoppingDistance;
    }

    /// <summary>
    /// 몬스터가 타겟을 바라보기 위한 메서드
    /// </summary>
    private void LookAtTarget()
    {
        if (m_targetTransform == null) return;

        var lookDirection = (m_targetTransform.position - transform.position).normalized;

        if (lookDirection != Vector3.zero)
        {
            lookDirection.y = 0;
            var lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    /// <summary>
    /// 타겟의 콜라이더 범위 중 몬스터와 가장 가까운 포인트와 몬스터 사이의 거리로 몬스터의 이동 여부를 판단
    /// </summary>
    private void HandleMoveAndStop(bool isTargetClose)
    {
        m_targetCollider = m_targetTransform?.GetComponent<Collider>();
        if (m_targetCollider != null)
        {
            if (isTargetClose)
            {
                LookAtTarget();

                m_navMeshAgent.isStopped = true;
                m_navMeshAgent.velocity = Vector3.zero;
                m_monsterAnimator.SetWalk(false);
                m_monsterState = MonsterState.Attack;
            }
            else
            {
                m_navMeshAgent.isStopped = false;
                m_monsterAnimator.SetWalk(true);
                m_monsterState = MonsterState.Walk;
            }
        }
    }

    private void HandleAttack(bool isTargetClose)
    {
        LookAtTarget();

        if (isTargetClose)
        {
            if (!GetRaycastHit()) return;

            m_monsterAnimator.SetAttack(true);
        }
        else
        {
            m_monsterAnimator.SetAttack(false);

            m_monsterState = MonsterState.Walk;
        }
    }

    /// <summary>
    /// 공격 시 피격을 당하는 타겟을 확인하기 위한 메서드
    /// </summary>
    /// <returns>타겟레이어가 hit 되었다면 true, 안되었다면 false를 반환</returns>
    private bool GetRaycastHit() =>
        Physics.Raycast(
            transform.position + Vector3.up,
            transform.forward,
            out Hit,
            MonsterDataSO.MonsterAtkRange,
            m_targetMask
        );

    /// <summary>
    /// Target을 찾기 위한 메서드
    /// 플레이어가 Fence 내에 있으면 Fence의 Position을 반환
    /// 플레이어가 Fence 밖에 있지만, 감지거리 밖에 있으면 Fence의 Position을 반환
    /// 플레이어가 감지거리 내에 있으면 Player의 Position을 반환
    /// </summary>
    /// <returns>타겟의 Transform을 반환</returns>
    private Transform FindTarget()
    {
        var targetTransform = m_targetTransform;

        //# 플레이어가 Fence에 있으면 Fence를 향해 걸어감
        if (IsPlayerStayInFence())
        {
            if (m_fenceTransform == null)
            {
                Debug.LogWarning($"{MonsterDataSO.MonsterEnName} - m_fence null");
                GameManager.Instance.SetGameOver();
                return null;
            }

            if (targetTransform != m_fenceTransform)
                m_monsterState = MonsterState.Walk;

            targetTransform = m_fenceTransform;
            return targetTransform;
        }

        //# 플레이어가 Fence 밖이면서 감지거리 밖에 있으면 Fence를 향해 걸어감
        if (!FindPlayer())
        {
            if (m_fenceTransform == null)
            {
                Debug.LogWarning($"{MonsterDataSO.MonsterEnName} - m_fence null");
                GameManager.Instance.SetGameOver();
                return null;
            }

            if (targetTransform != m_fenceTransform)
                m_monsterState = MonsterState.Walk;

            targetTransform = m_fenceTransform;
            return targetTransform;
        }

        if (targetTransform != m_playerTransform)
            m_monsterState = MonsterState.Walk;

        targetTransform = m_playerTransform;

        return targetTransform;
    }

    /// <summary>
    /// 몬스터와 플레이어의 거리를 계산하여 감지거리 내에 플레이어가 있는지 확인
    /// </summary>
    /// <returns>감지 거리 내라면 true, 아니면 false를 반환</returns>
    private bool FindPlayer()
    {
        if (m_playerTransform == null)
        {
            Debug.LogWarning($"{MonsterDataSO.MonsterEnName} - player null");
            GameManager.Instance.SetGameOver();
            return false;
        }
        return Vector3.Distance(transform.position, m_playerTransform.position) <= MonsterDataSO.MonsterDetectDistance;
    }

    /// <summary>
    /// Player가 Fence 내에 있는지 확인하는 메서드
    /// </summary>
    /// <returns>Fence 내에 있으면 true, Fence 내에 없으면 false를 반환</returns>
    private bool IsPlayerStayInFence()
    {
        var fenceXArea = GameManager.Instance.Tile.GetFenceXArea();
        var fenceYArea = GameManager.Instance.Tile.GetFenceYArea();

        if (m_playerTransform.position.x >= fenceXArea.x && m_playerTransform.position.x <= fenceXArea.y &&
            m_playerTransform.position.z >= fenceYArea.x && m_playerTransform.position.z <= fenceYArea.y) return true;

        return false;
    }

    /// <summary>
    /// 주기적으로 목적지를 target의 위치로 갱신
    /// </summary>
    private IEnumerator UpdatePath()
    {
        yield return WaitTime;

        while (!GameManager.Instance.IsGameOver)
        {
            m_navMeshAgent.SetDestination(m_targetTransform.position);
            yield return WaitTime;
        }
    }

    /// <summary>
    /// Legacy
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

    /// <summary>
    /// 몬스터 공격 애니메이션과 연동
    /// </summary>
    private void OnAttack()
    {
        if (Hit.collider == null) return;
        var target = Hit.collider.gameObject.GetComponent<IDamageable>();

        if (target == null) return;
        target?.TakenDamage(MonsterDataSO.MonsterAttack);
    }

    /// <summary>
    /// 몬스터 피격 애니메이션과 연동
    /// </summary>
    private void OnTakenDamaged()
    {
        m_navMeshAgent.isStopped = true;
        m_navMeshAgent.velocity = Vector3.zero;
        m_monsterAnimator.TriggerTakenDamage();
    }

    /// <summary>
    /// 몬스터 사망 애니메이션과 연공
    /// </summary>
    private void OnDie()
    {
        m_navMeshAgent.isStopped = true;
        m_navMeshAgent.velocity = Vector3.zero;
        m_monsterAnimator.TriggerToDie();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * MonsterDataSO.MonsterAtkRange);
    }
}