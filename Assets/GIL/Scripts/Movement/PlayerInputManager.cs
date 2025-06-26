using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private InteractionTestfromKSTtoGIL interact;
    public bool isSprinting;

    private float lookDelaytimer = 0.5f;
    private bool allowMove = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        interact = GetComponent<InteractionTestfromKSTtoGIL>();
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Sprint.started += ctx => motor.ActiveSprint();
        onFoot.Sprint.canceled += ctx => motor.DeactiveSprint();
        onFoot.Attack.performed += ctx => interact.MouseInteraction();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (allowMove)
        {
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>(), isSprinting);
        }
        else
        {
            lookDelaytimer -= Time.fixedDeltaTime;
            if (lookDelaytimer <= 0f) allowMove = true;
        }
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        if (allowMove)
        {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
        else
        {
            lookDelaytimer -= Time.fixedDeltaTime;
            if (lookDelaytimer <= 0f) allowMove = true;
        }
    }


    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
