using UnityEngine;

/// <summary>
/// 생성된 Monster를 등록하여 관리하기 위한 Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "New Monster Database", menuName = "Monster System/Monster Database")]
public class MonsterPrefabDatabaseSO : ScriptableObject
{
    public GameObject[] ModelPrefabObjects;
    public Mesh[] Meshs;

    public void OnSetPrefab(ref MonsterDataSO monsterDataSO)
    {
        for (int i = 0; i < ModelPrefabObjects.Length; i++)
        {
            if (!ModelPrefabObjects[i].name.Equals(monsterDataSO.MonsterEnName.ToString())) continue;

            monsterDataSO.ModelPrefab = ModelPrefabObjects[i];
            monsterDataSO.Mesh = Meshs[i];
        }

        if (monsterDataSO.ModelPrefab.Equals(null))
            Debug.LogWarning($"{monsterDataSO.MonsterEnName}과 일치하는 프리팹이 없습니다.");
    }
}