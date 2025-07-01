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
    private Interaction interact;
    private Inventory m_inventory;
    public bool isSprinting;

    private float lookDelaytimer = 0.5f;
    private bool allowMove = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        interact = GetComponent<Interaction>();
        onFoot.Jump.started += ctx => motor.Jump();
        onFoot.Sprint.started += ctx => motor.ActiveSprint();
        onFoot.Sprint.canceled += ctx => motor.DeactiveSprint();
        // onFoot.Attack.performed += ctx => interact.();

        m_inventory = GetComponent<Inventory>();
        onFoot.InventoryOpenClose.started += ctx => m_inventory.OnInventoryKeyPressed();
        onFoot.UIOpenClose.started += ctx => m_inventory.OnCloseKeyPressed();
        onFoot.UIOpenClose.started += ctx => FarmUI.OnCloseKeyPressed();
        onFoot.UIOpenClose.started += ctx => ExploitUI.OnCloseKeyPressed();
        onFoot.InventoryNumpad.started += m_inventory.OnNumpadKeyPressed;
        onFoot.InventoryPartial.started += ctx => InventoryDragHandler.OnPartialKeyPressed();
        onFoot.InventoryPartial.canceled += ctx => InventoryDragHandler.OnPartialKeyReleased();
        onFoot.Interaction.started += ctx => ItemPickUpInteraction.OnInteractionKeyPressed();
        onFoot.Interaction.canceled += ctx => ItemPickUpInteraction.OnInteractionKeyReleased();
        onFoot.Interaction.started += ctx => FarmUI.OnInteractionKeyPressed();
        onFoot.Interaction.canceled += ctx => FarmUI.OnInteractionKeyReleased();
        onFoot.Interaction.started += ctx => ExploitUI.OnInteractionKeyPressed();
        onFoot.Interaction.canceled += ctx => ExploitUI.OnInteractionKeyReleased();
        onFoot.Interaction.started += ctx => interact.OnInteractionKeyPressed();
        onFoot.Interaction.canceled += ctx => interact.OnInteractionKeyReleased();

        onFoot.LMBClick.started += ctx => interact.OnMouseButtonPressed();
        onFoot.LMBClick.canceled += ctx => interact.OnMouseButtonReleased();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        if (allowMove && GameManager.Instance.IsCursorLocked)
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
    private void LateUpdate()
    {
        if (allowMove && GameManager.Instance.IsCursorLocked)
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