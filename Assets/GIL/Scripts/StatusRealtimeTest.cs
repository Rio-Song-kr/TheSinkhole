using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRealtimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    private float mentalityDelta;
    private float hungerDelta;
    private bool isDay = true;
    void Start()
    {
        StartCoroutine(RealtimeSimulator());
        Debug.Log("낮으로 변경");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("낮으로 변경");
            isDay = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("밤으로 변경");
            isDay = false;
        }
    }

    private IEnumerator RealtimeSimulator()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            hungerDelta = isDay == true ? -0.025f : -0.01f;
            PlayerStatus.Instance.SetHunger(-0.025f);
            PlayerStatus.Instance.SetThirst(-0.1f);
            mentalityDelta = PlayerStatus.Instance.isStarving == true ? -0.05f : -0.025f;
            PlayerStatus.Instance.SetMentality(mentalityDelta);
            PlayerStatus.Instance.PrintAllCurStatus();
        }
    }
}
