using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusInstanceTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerStatus.Instance.SetHealth(-0.1f);
        }
    }
}
