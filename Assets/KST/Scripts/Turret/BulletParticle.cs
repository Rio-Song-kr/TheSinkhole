using UnityEngine;
using DesignPattern;
using System.Collections;

public class BulletParticle : PooledObject
{
    [SerializeField] private float lifeTime = 1.5f;
    [SerializeField]private ParticleSystem ps;



    public void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        ps?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // 이전 파티클 남는 문제 방지
        ps?.Play();
        StartCoroutine(ReturnPoolDelay(lifeTime));
    }

    private IEnumerator ReturnPoolDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false); // 비활성화로 풀에 반납
        ReturnPool();
    }


}