using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusInstanceTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerStatus.Instance.SetHealth(+0.1f);
            Debug.Log($"현재 체력 : {PlayerStatus.Instance.CurHealth}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerStatus.Instance.SetHunger(0.1f);
            Debug.Log($"현재 배고픔 : {PlayerStatus.Instance.CurHunger} 현재 이동속도 : {PlayerStatus.Instance.CurPlayerMoveSpeed} 현재 행동속도 : {PlayerStatus.Instance.ActionSpeed}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerStatus.Instance.SetThirst(+0.1f);
            Debug.Log($"현재 갈증 : {PlayerStatus.Instance.CurThirst}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayerStatus.Instance.SetMentality(+0.1f);
            Debug.Log($"현재 정신력 : {PlayerStatus.Instance.CurMentality}");
        }
    }
}
