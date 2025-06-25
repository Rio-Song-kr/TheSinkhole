using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    protected PlayerStatus player;
    protected Coroutine coroutine;

    public Debuff(PlayerStatus playerStatus)
    {
        player = playerStatus;
    }

    public void StartDebuff(MonoBehaviour mono)
    {
        if (coroutine == null)
            coroutine = mono.StartCoroutine(ApplyDebuff());
    }

    public void StopDebuff(MonoBehaviour mono)
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
            ResetEffect();
            coroutine = null;
        }
    }

    protected abstract IEnumerator ApplyDebuff();
    protected abstract void ResetEffect();
}
