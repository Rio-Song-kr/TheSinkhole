using UnityEngine;

public class MonsterPool<T> where T : SceneMonster
{
    public PoolManager<T> Pool;

    public void SetPool(GameObject prefab)
    {
        var component = prefab.GetComponent<T>();
        Pool = new PoolManager<T>(component);
    }
}