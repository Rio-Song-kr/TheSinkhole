using System;
using UnityEngine;

public class TestPlayerTool : MonoBehaviour
{
    public ToolType currentToolType = ToolType.None;

    void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentToolType = ToolType.None; // 아무것도 x
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentToolType = ToolType.Pick; //곡괭이
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentToolType = ToolType.Shovel; // 삽
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentToolType = ToolType.Hoe; //괭이
        }

    }
}