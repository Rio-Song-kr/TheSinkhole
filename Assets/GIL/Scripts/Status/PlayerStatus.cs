using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStatus : MonoBehaviour, IDamageable
{
    public static PlayerStatus Instance { get; set; }
    [Tooltip("값을 변화하고 싶을 땐 PlayerStatus.Instance.Set스텟명(변화량 퍼센트) 사용")]
    public const int StatId = 10000;
    public const int RealtimeOneMinute = 60;
    // UI를 고려하여 최대체력과 현재 체력을 두개 보유
    // Slider.value 혹은 Image.FillAmount 방식을 사용해야 할 때를 고려
    // Max스탯 : 최대 스탯들
    // Cur스탯 : 현재 스탯들 <- Set스탯을 사용할 경우 이것들이 변화함.
    // 체력
    public float MaxHealth;

    [field: SerializeField] public float CurHealth { get; private set; }
    // 배고픔
    [Header("Hunger")]
    public float MaxHunger;
    [field: SerializeField] public float CurHunger { get; private set; }
    [SerializeField] private float moveSpeedDebuffStat = 0.5f;
    [SerializeField] private float actionSpeedDebuffStat = 2f;
    [Header("Debuff")]
    public bool isStarving;
    private bool isDehydrated;
    private StarvationDebuff starvationDebuff;
    private DehydrationDebuff dehydrationDebuff;

    private Inventory m_inventory;
    private InteractionUIManager m_uiManager;
    private bool m_isConsumableItem;

    // 갈증
    [Header("Thirst")]
    public float MaxThirst;

    [field: SerializeField] public float CurThirst { get; private set; }
    // 정신력
    [Header("Mentality")]
    public float MaxMentality;

    [field: SerializeField] public float CurMentality { get; private set; }

    [field: SerializeField] public int AttackPower { get; private set; }

    [Header("Speed")]
    // 이동속도
    // TODO: 추후에 이동 구현 시 이동속도 관련 코드에다가 옮길지 고민하기.
    public float MaxPlayerMoveSpeed = 2;
    public float CurPlayerMoveSpeed;
    // 행동속도
    // 추후에 행동을 할 때 속도 배율로 사용할 예정.
    public float ActionSpeed = 1;
    // 공격속도
    // 추후에 공격등을 할 때 배율로 사용할 예정.
    public float AtkSpeed = 1;

    [Header("Scene Loader")]
    [SerializeField] private float m_flareTime = 3f;
    [SerializeField] private string m_sceneName = "OutroScene";

    public GameObject panel;
    private Coroutine m_panelCoroutine;

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

        starvationDebuff = new StarvationDebuff(this, moveSpeedDebuffStat, actionSpeedDebuffStat);
        dehydrationDebuff = new DehydrationDebuff(this);

        m_inventory = GetComponent<Inventory>();
        m_uiManager = GetComponent<InteractionUIManager>();

        Inventory.OnSelectedItemChanged += (param) => OnSelectedItemChanged();
        GameManager.Instance.Action.OnActionEffect += OnActionEffect;
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
    /// <param name="value">변화할 배고픔 스텟의 퍼센트 float값</param>
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
                starvationDebuff.StartDebuff(this);
                isStarving = true;
            }
        }
        else if (isStarving)
        {
            starvationDebuff.StopDebuff(this);
            isStarving = false;
        }
    }
    /// <summary>
    /// 갈증 스텟의 변화 퍼센트, 갈증이 0이 될 경우 탈수 디버프(이동속도 -50% , 행동속도 -50%)발동
    /// </summary>
    /// <param name="value">변화할 갈증의 퍼센트 float값</param>
    // 갈증 스텟이 0이 될 경우 탈수 디버프 시작
    // 갈증 스텟이 0이상이 될 경우 탈수 디버프 즉시 해체
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
                dehydrationDebuff.StartDebuff(this);
                isDehydrated = true;
            }
        }
        else if (isDehydrated)
        {
            dehydrationDebuff.StopDebuff(this);
            isDehydrated = false;
        }
    }
    /// <summary>
    /// 정신력 수치의 변화, 정신력이 0이 될 경우 체력이 0으로 설정, 사망
    /// </summary>
    /// <param name="value">변화할 정신력의 퍼센트 float값</param>
    public void SetMentality(float value)
    {
        float deltaValue = MaxMentality * value;
        CurMentality += deltaValue;
        CurMentality = Mathf.Clamp(CurMentality, 0f, MaxMentality);
        // 정신력이 0이 될 경우 발생할 로직
        if (CurMentality <= 0f)
        {
            SetHealth(-1f);
        }
    }
    // 플레이어 사망시 처리되는 로직들
    public void PlayerDeath()
    {
        GameManager.Instance.SetGameOver();
        Debug.Log("플레이어 사망!");
    }

    /// <summary>
    /// 리얼 타임 사이클에 따른 스탯 변화를 모아둔 함수
    /// 낮/밤에 따른 배고픔 게이지 하락 속도,
    /// 허기 디버프에 따른 정신력 감소 속도가 다름
    /// </summary>
    /// <param name="isDay">낮인지 확인하는 파라미터, true = 낮, false = 밤</param>
    public void RealtimeStatusCycle(bool isDay)
    {
        // 추후에 낮 or 밤 판정에 따라 변경할 수 있게 함.
        float hungerDelta = isDay == true ? -0.025f : -0.01f;
        SetHunger(hungerDelta);
        SetThirst(-0.1f);
        float mentalityDelta = isStarving == true ? -0.05f : -0.025f;
        SetMentality(mentalityDelta);
    }
    // [Test] 필요 없어질 경우 바로 지울 것.
    // 플레이어의 현재 스텟들을 전부 출력하는 로직
    public void PrintAllCurStatus()
    {
        Debug.Log(
            $"체력: {CurHealth}, 배고픔: {CurHunger}, 갈증: {CurThirst}, 정신력: {CurMentality}, 이동속도: {CurPlayerMoveSpeed}, 행동속도: {ActionSpeed}");
    }
    // 허기 디버프, 이동속도가 반으로 감소하고, 행동속도가 2배 증가하는
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

    /// <summary>
    /// 정수형 데미지를 최대 체력의 비율로 전환하여 비율만큼 깎음
    /// </summary>
    /// <param name="damage"></param>
    public void TakenDamage(int damage)
    {
        // damage가 최대 체력의 몇 퍼센트인지 계산
        // 그만큼 SetHealth를 한다.
        panel.SetActive(true);

        if (m_panelCoroutine == null)
            m_panelCoroutine = StartCoroutine(OnDisableHitPanel());
        float percent = damage / MaxHealth;
        SetHealth(-percent);
    }

    private IEnumerator OnDisableHitPanel()
    {
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(false);
        m_panelCoroutine = null;
    }

    /// <summary>
    /// SelectItem이 ConsumableItem일 때 할 효과
    /// </summary>
    private void OnSelectedItemChanged()
    {
        //# 소비 가능한 아이템이 아닌 경우 return
        if (m_inventory.GetQuickSlotItemType() != ItemType.ConsumableItem)
        {
            m_uiManager.ClearInteractionUI(InteractionType.ConsumableItem);
            m_isConsumableItem = false;
            return;
        }

        m_uiManager.SetInteractionUI(
            InteractionType.ConsumableItem, true, "아이템을 사용하려면 [E]를 눌러주세요", false
        );

        m_isConsumableItem = true;
    }

    private void HandleItemEffect()
    {
        if (!m_isConsumableItem) return;

        //# 소비 가능한 아이템인 경우, ItemDataSO를 가져옴
        var itemData = m_inventory.GetQuickSlotItemData();
        var itemEffect = itemData.ItemData.ItemEffect;

        if(itemData.ItemData.ItemId == 20302)
        {
            StartCoroutine(LoadOutroScene());
        }

        HandleEffect(itemEffect);

        m_inventory.RemoveItemAmounts(itemData.ItemEnName, 1);
        // m_uiManager.ClearInteractionUI(InteractionType.ConsumableItem);
        // m_isConsumableItem = false;
    }

    private IEnumerator LoadOutroScene()
    {
        yield return new WaitForSeconds(m_flareTime);
        SceneManager.LoadScene(m_sceneName);
    }

    private void HandleEffect(EffectFileData effect)
    {
        switch (effect.Type)
        {
            case StatusType.Hp:
                SetHealth(effect.StatusAmount);
                break;
            case StatusType.Hungry:
                SetHunger(effect.StatusAmount);
                break;
            case StatusType.Mental:
                SetMentality(effect.StatusAmount);
                break;
            case StatusType.Thirst:
                SetThirst(effect.StatusAmount);
                break;
        }
    }

    private void OnActionEffect(List<int> effectIds)
    {
        foreach (int effectId in effectIds)
        {
            HandleEffect(GameManager.Instance.Effect.EffectIdData[effectId]);
        }
    }

    public void OnInteractionKeyPressed() => HandleItemEffect();

    public void OnRealTimeEffect()
    {
        //# 배고픔
        var hungerEffect = GameManager.Instance.Effect.EffectIdData[35007];

        SetHunger(hungerEffect.StatusAmount);

        //# 갈증
        var thirstyEffect = GameManager.Instance.Effect.EffectIdData[35008];
        SetThirst(thirstyEffect.StatusAmount);

        //# 멘탈
        var mentalityEffect = GameManager.Instance.Effect.EffectIdData[35009];
        SetMentality(mentalityEffect.StatusAmount);
    }
}