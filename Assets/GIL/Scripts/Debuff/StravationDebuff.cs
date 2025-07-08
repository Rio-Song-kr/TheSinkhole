using System;
using System.Collections;
using UnityEngine;

public class StarvationDebuff : Debuff
{
    private float moveSpeedDebuff;
    private float actionSpeedMultiplier;

    public StarvationDebuff(PlayerStatus playerStatus, float moveSpeedDebuff, float actionSpeedMultiplier)
        : base(playerStatus)
    {
        this.moveSpeedDebuff = moveSpeedDebuff;
        this.actionSpeedMultiplier = actionSpeedMultiplier;
    }

    protected override IEnumerator ApplyDebuff()
    {
        Debug.Log("굶주렸다..");
        player.CurPlayerMoveSpeed -= player.MaxPlayerMoveSpeed * moveSpeedDebuff;
        player.ActionSpeed *= actionSpeedMultiplier;

        while (true)
        {
            player.SetHealth(-0.2f);
            yield return new WaitForSecondsRealtime(PlayerStatus.RealtimeOneMinute);
        }
    }

    protected override void ResetEffect()
    {
        player.CurPlayerMoveSpeed = player.MaxPlayerMoveSpeed;
        player.ActionSpeed = 1f;
    }
}