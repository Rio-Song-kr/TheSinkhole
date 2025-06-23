using System;
using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; set; }
    [Tooltip("값을 변화하고 싶을 땐 PlayerStatus.Instance.Set스텟명(변화량 퍼센트) 사용")]
    [Header("Attributes")]
    public const int StatId = 10000;
    public const int RealtimeOneMinute = 60;
    // UI를 고려하여 최대체력과 현재 체력을 두개 보유
    // Slider.value 혹은 Image.FillAmount 방식을 사용해야 할 때를 고려
    // 체력
    public float MaxHealth;
    public float CurHealth;
    // 배고픔
    [Header("Hunger")]
    public float MaxHunger;
    public float CurHunger;
    [SerializeField] float moveSpeedDebuffStat = 0.5f;
    [SerializeField] float actionSpeedDebuffStat = 2f;
    private bool isStarving;
    private Coroutine starvationCoroutine;
    private bool isDehydrated;
    private Coroutine dehydrationCoroutine;

    // 갈증
    [Header("Thirst")]
    public float MaxThirst;
    public float CurThirst;
    // 정신력
    [Header("Mentality")]
    public float MaxMentality;
    public float CurMentality;
    [Header("Speed")]
    // 이동속도
    // TODO: 추후에 이동 구현 시 이동속도 관련 코드에다가 옮길지 고민하기.
    public float MaxPlayerMoveSpeed = 2;
    public float CurPlayerMoveSpeed;
    // 행동속도
    // 추후에 행동을 할 때 속도 배율로 사용할 예정.
    public float ActionSpeed = 1;

    // 싱글톤 검정
    private void Awake() => Init();
    // 현재 스텟을 최대 스탯과 동일한 값으로 지정
    private void Start()
    {
        CurHealth = MaxHealth;
        CurHunger = MaxHunger;
        CurThirst = MaxThirst;
        CurMentality = MaxMentality;
        CurPlayerMoveSpeed = MaxPlayerMoveSpeed;
    }

    /// <summary>
    /// 체력 스텟의 변화 퍼센트, 체력이 0이 될경우 플레이어 사망
    /// </summary>
    /// <param name="value">변화할 체력의 퍼센트 float값</param>
    // 능력치들이 전부 퍼센트로 변화하기 때문에 이 방식을 채택
    // 체력을 20퍼 깍는다 -> 최대 체력에 -0.2를 곱한 다음 그걸 현재 체력에서 더하기.
    // 100에 PlayerStatus.SetHealth(-0.2)를 쓸 경우 100 + 100 * -0.2 --> 100 - 20 = 80
    public void SetHealth(float value)
    {
        float deltaValue = MaxHealth * value;
        CurHealth += deltaValue;
        CurHealth = Math.Clamp(CurHealth, 0f, MaxHealth);
        // 체력이 0이 될 경우 발생할 로직
        if (CurHealth <= 0f) PlayerDeath();
    }
    /// <summary>
    /// 배고픔 스텟의 변화 퍼센트, 배고픔이 0이 될 경우 허기 디버프(이동속도 -50% , 행동속도 -50%)발동
    /// </summary>
    /// <param name="value">변화할 체력의 퍼센트 float값</param>
    // 배고픔이 0 이하가 될 경우 허기 디버프 발동
    // 배고픔이 다시 0 이상이 될 경우 허기 디버프 해체.
    public void SetHunger(float value)
    {
        float deltaValue = MaxHunger * value;
        CurHunger += deltaValue;
        CurHunger = Mathf.Clamp(CurHunger, 0f, MaxHunger);
        // 배고픔이 0이 될 경우 발생할 로직
        if (CurHunger <= 0f)
        {
            if (!isStarving)
            {
                starvationCoroutine = StartCoroutine(StarvationDebuff(moveSpeedDebuffStat, actionSpeedDebuffStat));
                isStarving = true;
            }
        }
        else if (isStarving)
        {
            if (starvationCoroutine != null) StopCoroutine(starvationCoroutine);
            CurPlayerMoveSpeed = MaxPlayerMoveSpeed;
            ActionSpeed = 1f;
            isStarving = false;
        }
    }
    /// <summary>
    /// 갈증 스텟의 변화 퍼센트, 갈증이 0이 될 경우 탈수 디버프(이동속도 -50% , 행동속도 -50%)발동
    /// </summary>
    /// <param name="value">변화할 체력의 퍼센트 float값</param>
    public void SetThirst(float value)
    {
        float deltaValue = MaxThirst * value;
        CurThirst += deltaValue;
        CurThirst = Mathf.Clamp(CurThirst, 0f, MaxThirst);
        // 갈증이 0이 될 경우 발생할 로직
        if (CurThirst <= 0f)
        {
            if (!isDehydrated)
            {
                dehydrationCoroutine = StartCoroutine(DehydrationDebuff());
                isDehydrated = true;
            }
        }
        else if (isDehydrated)
        {
            if (dehydrationCoroutine != null) StopCoroutine(dehydrationCoroutine);
            isDehydrated = false;
        }
    }
    /// <summary>
    /// 정신력 수치의 변화, 정신력이 0이 될 경우 체력이 0으로 설정, 사망
    /// </summary>
    /// <param name="value">변화할 체력의 퍼센트 float값</param>
    public void SetMentality(float value)
    {
        float deltaValue = MaxHealth * value;
        CurMentality += deltaValue;
        CurMentality = Mathf.Clamp(CurMentality, 0f, MaxMentality);
        // 정신력이 0이 될 경우 발생할 로직
        if (CurMentality <= 0f)
        {
            SetHealth(0f);
        }
    }
    // 플레이어 사망시 처리되는 로직들
    public void PlayerDeath()
    {
        Debug.Log("플레이어 사망!");
    }
    // 허기 디버프, 이동속도가 반으로 감소하고, 행동속도가 2배 증가하는
    private IEnumerator StarvationDebuff(float moveSpeed, float actionSpeed)
    {
        Debug.Log("굶주렸다..");
        CurPlayerMoveSpeed -= MaxPlayerMoveSpeed * moveSpeed;
        ActionSpeed *= actionSpeed;
        while (true)
        {
            SetHealth(-0.2f);
            yield return new WaitForSecondsRealtime(RealtimeOneMinute);
        }
    }

    private IEnumerator DehydrationDebuff()
    {
        Debug.Log("목이 마르다..");
        while (true)
        {
            SetHealth(-0.1f);
            yield return new WaitForSecondsRealtime(RealtimeOneMinute);
        }
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
