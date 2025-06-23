using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; set; }
    [Tooltip("값을 변화하고 싶을 땐 PlayerStatus.Instance.Set스텟명(변화량 퍼센트) 사용")]
    [Header("Attributes")]
    public const int StatId = 10000;
    // UI를 고려하여 최대체력과 현재 체력을 두개 보유
    // Slider.value 혹은 Image.FillAmount 방식을 사용해야 할 때를 고려
    // 체력
    public float MaxHealth;
    public float CurHealth;
    // 배고픔
    public float MaxHunger;
    public float CurHunger;
    // 갈증
    public float MaxThirst;
    public float CurThirst;
    // 정신력
    public float MaxMentality;
    public float CurMentality;

    // 싱글톤 검정
    private void Awake() => Init();
    // 현재 스텟을 최대 스탯과 동일한 값으로 지정
    private void Start()
    {
        CurHealth = MaxHealth;
        CurHunger = MaxHunger;
        CurThirst = MaxThirst;
        CurMentality = MaxMentality;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// <summary>
    /// 플레이어 체력에 변화, 체력이 0이 될경우 플레이어 사망
    /// </summary>
    /// <param name="value">변화할 체력의 퍼센트 float값</param>
    // 능력치들이 전부 퍼센트로 변화하기 때문에 이 방식을 채택
    // 체력을 20퍼 깍는다 -> 최대 체력에 -0.2를 곱한 다음 그걸 현재 체력에서 더하기.
    // 100에 PlayerStatus.SetHealth(-0.2)를 쓸 경우 100 + 100 * -0.2 --> 100 - 20 = 80
    public void SetHealth(float value)
    {
        float deltaValue = MaxHealth * value;
        CurHealth += deltaValue;
        if (CurHealth >= MaxHealth) CurHealth = MaxHealth;
        if (CurHealth <= 0) PlayerDeath();
        Debug.Log(CurHealth);
    }

    // 배고픔을 관리하는 로직
    //public void Set
    // 갈증을 관리하는 로직
    // 정신력을 관리하는 로직
    // 플레이어 사망시 처리되는 로직들
    public void PlayerDeath()
    {
        Debug.Log("플레이어 사망!");
    }

    private void Init()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
