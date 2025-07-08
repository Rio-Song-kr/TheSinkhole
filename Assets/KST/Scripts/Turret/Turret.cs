using System.Collections;
using UnityEngine;
using DesignPattern;
using System;
using Util;

public class Turret : MonoBehaviour
{
    //TODO: Util의 Stat사용하기

    [Header("스탯")]
    //인스펙터에서 초기화 하기 위해.
    // [SerializeField] private int level;
    // [SerializeField] private int maxLevel;
    // [SerializeField] private int initMaxHP;
    // [SerializeField] private int initHP;
    [SerializeField] private int dmg;

    // public Stat<int> HP { get; private set; }
    // public Stat<int> MaxHP { get; private set; }
    public Stat<int> Damge { get; private set; }
    // public Stat<int> Level { get; private set; }
    // public Stat<int> MaxLevel { get; private set; }



    [SerializeField] private Transform shootingTransform;

    [SerializeField] private float delayTime = 0.3f;
    YieldInstruction fireDelay;
    [SerializeField] private TurretSo turretSo;

    public Transform targetTransform;
    public Coroutine shootCoroutine;
    [SerializeField] private Bullet bulletPrefab;
    private ObjectPool bulletPool;
    private ObjectPool bulletParticlePool;
    [SerializeField] private BulletParticle particlePrefab;

    [SerializeField] private AudioClip towerAttackSFX;
    [SerializeField] private float testDamage;

    // public TriggerTurret towerZone;

    void Awake()
    {
        // HP = new(initHP);
        // MaxHP = new(initMaxHP);
        Damge = new(dmg);
        // Level = new(level);
        // MaxLevel = new(maxLevel);
    }
    void Start()
    {
        fireDelay = new WaitForSeconds(delayTime);

        bulletPool = new ObjectPool(null, bulletPrefab, 10);

        bulletParticlePool = new ObjectPool(null, particlePrefab, 10);
    }

    void Update()
    {
        if (targetTransform != null)
            RotateTurret();
    }

    void OnDestroy()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
        targetTransform = null;
    }

    public void Init(TurretSo so)
    {
        turretSo = so;
        Damge = new Stat<int>(turretSo.Atk);
        testDamage = Damge.Value;
        

        var trigger = GetComponentInChildren<TurretDectTrigger>();
        if (trigger != null)
        {
            trigger.SetDistance(turretSo.distance);
        }
        Debug.Log($"공격력 {testDamage}");

    }

    private void RotateTurret()
    {
        Vector3 target = new(targetTransform.position.x, transform.position.y, targetTransform.position.z);
        transform.LookAt(target);
    }

    public IEnumerator CoShoot()
    {
        while (targetTransform != null)
        {
            if (targetTransform != null)
            {
                yield return fireDelay;
                Shoot();
            }
        }
        shootCoroutine = null;
    }

    private void Shoot()
    {
        if (bulletPrefab == null || shootingTransform == null || targetTransform == null)
            return;
        Vector3 targetPos = targetTransform.position + Vector3.up * 1f;
        Vector3 direction = targetPos - shootingTransform.position;
        Bullet bullet = bulletPool.PopPool() as Bullet;
        bullet.transform.position = shootingTransform.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        // bullet.BulletInit(direction, bulletParticlePool);
        bullet.BulletInit(direction, this);

        PlayShootSFX();
    }

    private void PlayShootSFX()
    {
        // SFXController sfx = GameManager.Instance.Audio.GetSFX();
        // SFXController sfx = AudioManager.Instance.GetSFX();
        // sfx.Play(towerAttackSFX);
    }

    public void SpawnEffect(Vector3 _position)
    {
        var effect = bulletParticlePool.PopPool() as BulletParticle;
        effect?.Activate(_position);
        // if (effect != null)
        //     effect.Activate(position);

    }

    // public void TowerDestory()
    // {
    //     SpawnEffect(transform.position);
    //     towerZone.TowerDestoried();
    //     Destroy(gameObject);
    // }

    // public void LevelUp()
    // {
    //     if (Level.Value == MaxLevel.Value) return;
    //     Level.Value += 1;
    //     Damge.Value += 1;
    //     MaxHP.Value += 5;
    //     HP.Value = MaxHP.Value;
    // }
}
