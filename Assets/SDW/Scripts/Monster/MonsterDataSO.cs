using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Monster System/Monster")]
public class MonsterDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string MonsterName;
    public MonsterEnName MonsterEnName;
    public MonsterTierType MonsterTierType;
    public MonsterAttackType MonsterAttackType;
    public int MaxMonsterHP;
    public int MaxMonsterSpeed;
    public int MonsterAttack;
    public float MonsterAtkSpeed;
    public float MonsterAtkRange;
    public float MonsterResearch;
    [TextArea(4, 4)] public string MonsterDescription;

    [Header("2D Icon")]
    public Sprite Icon;

    [Header("Model")]
    public GameObject ModelPrefab;

    [Header("Data")]
    [SerializeReference] public Monster MonsterData;
}