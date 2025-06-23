using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusInstanceTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStatus.Instance.SetHealth(+0.1f);
            Debug.Log($"현재 체력 : {PlayerStatus.Instance.CurHealth}");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerStatus.Instance.SetHealth(-0.1f);
            Debug.Log($"현재 체력 : {PlayerStatus.Instance.CurHealth}");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerStatus.Instance.SetHunger(0.1f);
            Debug.Log($"현재 배고픔 : {PlayerStatus.Instance.CurHunger} 현재 이동속도 : {PlayerStatus.Instance.CurPlayerMoveSpeed} 현재 행동속도 : {PlayerStatus.Instance.ActionSpeed}");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerStatus.Instance.SetHunger(-0.1f);
            Debug.Log($"현재 배고픔 : {PlayerStatus.Instance.CurHunger} 현재 이동속도 : {PlayerStatus.Instance.CurPlayerMoveSpeed} 현재 행동속도 : {PlayerStatus.Instance.ActionSpeed}");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerStatus.Instance.SetThirst(+0.1f);
            Debug.Log($"현재 갈증 : {PlayerStatus.Instance.CurThirst}");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerStatus.Instance.SetThirst(-0.1f);
            Debug.Log($"현재 갈증 : {PlayerStatus.Instance.CurThirst}");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerStatus.Instance.SetMentality(+0.1f);
            Debug.Log($"현재 정신력 : {PlayerStatus.Instance.CurMentality}");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerStatus.Instance.SetMentality(-0.1f);
            Debug.Log($"현재 정신력 : {PlayerStatus.Instance.CurMentality}");
        }
    }
}
