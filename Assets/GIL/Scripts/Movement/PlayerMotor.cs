using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInputManager playerInput;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;
    public float sprintingSpeed = 2f;
    public float speed;
    // TODO : [Test] 추후에 개발이 완성되면 지울 것!
    public TextMeshProUGUI curMoveVelocityText;
    private void Awake()
    {
        speed = PlayerStatus.Instance.CurPlayerMoveSpeed;
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInputManager>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        isGrounded = controller.isGrounded;
        speed = PlayerStatus.Instance.CurPlayerMoveSpeed;
    }
    public void ActiveSprint()
    {
        speed *= sprintingSpeed;
        playerInput.isSprinting = true;
    }
    public void DeactiveSprint()
    {
        speed *= 1f;
        playerInput.isSprinting = false;
    }
    public void ProcessMove(Vector2 input, bool isSprinting)
    {
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        Vector3 worldMoveDir = transform.TransformDirection(moveDir);
        float currentSpeed = isSprinting && !PlayerStatus.Instance.isStarving ? speed * sprintingSpeed : speed;
        Vector3 moveVelocity = worldMoveDir * currentSpeed;
        // TODO : [Test] 추후에 개발이 완성되면 지울 것!
        if (curMoveVelocityText != null)
        {
            float horizontalSpeed = new Vector3(moveVelocity.x, 0f, moveVelocity.z).magnitude;
            curMoveVelocityText.text = $"Speed: {horizontalSpeed:F2}";
        }
        controller.Move(moveVelocity * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
