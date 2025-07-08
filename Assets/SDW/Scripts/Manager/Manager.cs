using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
    //todo 추후 통합시 사용
    public static GameManager GameManager => GameManager.Instance;
    //
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameManager.CreateInstance();
    }
}