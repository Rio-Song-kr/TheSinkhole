using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatRealtimeTest : MonoBehaviour
{
    // Start is called before the first frame update
    public float RealTimeTickSeconds = 2f;
    private float mentalityDelta;
    private float hungerDelta;
    private bool isDay = true;
    void Start()
    {
        StartCoroutine(RealtimeSimulator());
    }

    private IEnumerator RealtimeSimulator()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(RealTimeTickSeconds);
            PlayerStatus.Instance.RealtimeStatusCycle(isDay);
        }
    }
}
