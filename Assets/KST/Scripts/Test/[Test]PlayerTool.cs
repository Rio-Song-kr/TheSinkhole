using System;
using UnityEngine;

public class TestPlayerTool : MonoBehaviour
{
    public ToolType CurrentToolType = ToolType.None;
    [SerializeField] private Interaction m_interaction;

    void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentToolType = ToolType.None; // 아무것도 x
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentToolType = ToolType.Pick; //곡괭이
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentToolType = ToolType.Shovel; // 삽
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentToolType = ToolType.Hammer; //괭이
        }

        m_interaction.SetCurrentTool(CurrentToolType);
    }
}