using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
