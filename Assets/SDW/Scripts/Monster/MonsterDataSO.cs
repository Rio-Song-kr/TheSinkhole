using UnityEngine;

/// <summary>
/// Game에서 사용할 Monster의 데이터를 포함하고, Icon, Prefab 등을 정의하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Monster", menuName = "Monster System/Monster")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string MonsterName;
    public MonsterEnName MonsterEnName;
    public MonsterTierType MonsterTierType;
    public MonsterAttackType MonsterAttackType;
    public int MaxMonsterHealth;
    public int MaxMonsterSpeed;
    public int MonsterAttack;
    public float MonsterAtkSpeed;
    public float MonsterAtkRange;
    public float MonsterDetectDistance;
    public int MonsterDropItemId;
    public int MonsterDropItemQuantity;
    [TextArea(4, 4)] public string MonsterDescription;

    [Header("2D Icon")]
    public Sprite Icon;

    [Header("Model")]
    public GameObject ModelPrefab;

    [Header("Data")]
    [SerializeReference] public Monster Monster;
}