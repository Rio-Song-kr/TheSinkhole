using System;
using System.Collections;
using UnityEngine;

public class DehydrationDebuff : Debuff
{
    public DehydrationDebuff(PlayerStatus playerStatus) : base(playerStatus) { }

    protected override IEnumerator ApplyDebuff()
    {
        Debug.Log("목이 마르다..");
        while (true)
        {
            player.SetHealth(-0.1f);
            yield return new WaitForSecondsRealtime(PlayerStatus.RealtimeOneMinute);
        }
    }

    protected override void ResetEffect()
    {
        // 탈수 디버프는 스탯 영향 없음
    }
}