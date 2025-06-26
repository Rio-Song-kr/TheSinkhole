using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager Instance => m_instance;

    public GlobalUIManager UI { get; private set; }

    //todo 추후 통합시 아용
    // public static void CreateInstance()
    // {
    //     if (_instance == null)
    //     {
    //         var gameManagerPrefab = Resources.Load<GameManager>("GameManager");
    //         _instance = Instantiate(gameManagerPrefab);
    //         DontDestroyOnLoad(_instance);
    //     }
    // }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
        DontDestroyOnLoad(gameObject);

        UI = GetComponent<GlobalUIManager>();
    }
}