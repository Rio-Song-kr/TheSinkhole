using UnityEngine;
using DesignPattern;
using Test;

public class Bullet : PooledObject
{
    // private TowerTest tower;
    private Vector3 _direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifeTime = 3f;

    [SerializeField] private float _timer;
    Rigidbody rb;
    Turret tower;
    // ObjectPool particlePool;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _timer = 0f;
        enabled = true;
    }
    // public void BulletInit(Vector3 direction, ObjectPool effectpool)
    public void BulletInit(Vector3 direction, Turret _tower)
    {
        _direction = direction.normalized;
        _timer = 0f; // 수명 타이머 초기화
        // particlePool = effectpool;
        tower = _tower;

        //리지드 바디 초기화(안 해주면 계속 속도 누적됨.)
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = transform.position;
            rb.rotation = transform.rotation;
        }

    }

    void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            tower.SpawnEffect(transform.position);
            ReturnPool(); // 일정 시간 지나면 풀로 반환
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log("총알이 몬스터에게 부딪침");
            // var effect = particlePool?.PopPool() as BulletParticle;
            // effect?.Activate(transform.position);
            tower.SpawnEffect(transform.position);


            // 데미지 처리
            // MonsterController monster = other.GetComponent<MonsterController>();
            // monster.TakeDamage(tower.Damge.Value);
            
            ReturnPool(); // 풀로 반환
        }
    }
}