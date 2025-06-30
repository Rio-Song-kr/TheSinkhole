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
    private Vector3 moveVelocity;

    [Header("Movement")]
    public float gravity = -9.8f;
    public float jumpSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float sprintingSpeed = 2f;
    [Header("Slope Sliding")]
    public float slopeRayRange = 0.6f;
    public float slideSpeed = 3f;
    public float movementSpeed;

    // 빗면
    public float slopeDownwardForce = -10f;
    public bool isSliding = false;
    private Vector3 worldMoveDir;
    private Vector3 adjustedMove;

    // 점프
    private bool isJumping = false;

    // TODO : [Test] 추후에 개발이 완성되면 지울 것!
    [Header("Test")]

    #region [Test]

    public TextMeshProUGUI curMoveVelocityText;
    public GameObject panel;

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInputManager>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        isGrounded = controller.isGrounded;
        movementSpeed = PlayerStatus.Instance.CurPlayerMoveSpeed;
    }

    /// <summary>
    /// 플레이어 움직임을 제어
    /// 스탯창의 이동속도를 바탕으로 움직이는 속도 구현
    /// 기본 설정으론 Shift키를 눌러 달리기를 진행할 수 있음
    /// </summary>
    /// <param name="input"></param>
    /// <param name="isSprinting"></param>
    public void ProcessMove(Vector2 input, bool isSprinting)
    {
        var moveDir = new Vector3(input.x, 0, input.y);
        worldMoveDir = transform.TransformDirection(moveDir);
        Debug.DrawLine(transform.position, worldMoveDir * 2f, Color.red);

        GetSlopeMovement();

        float currentSpeed = isSprinting && !PlayerStatus.Instance.isStarving
            ? movementSpeed * sprintingSpeed
            : movementSpeed;

        moveVelocity = adjustedMove * currentSpeed;

        if (isGrounded && playerVelocity.y < 0f)
            playerVelocity.y = -3.0f;
        else
            playerVelocity.y += gravity * jumpSpeed * Time.deltaTime;

        var totalVelocity = moveVelocity + playerVelocity;
        controller.Move(totalVelocity * Time.deltaTime);

        // TODO : [Test] 추후에 개발이 완성되면 지울 것!

        #region [Test]

        float horizontalSpeed = new Vector3(moveVelocity.x, 0f, moveVelocity.z).magnitude;
        if (curMoveVelocityText != null) curMoveVelocityText.text = $"Speed: {horizontalSpeed:F2}";

        #endregion
    }

    /// <summary>
    /// 플레이어가 점프를 하는 기능
    /// </summary>
    public void Jump()
    {
        if (isGrounded && isSliding == false)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity * jumpSpeed);
        }
    }

    /// <summary>
    /// 플레이어가 달릴 경우 발동되는 기능들
    /// </summary>
    // 추후에 이펙트 or UI 반응을 추가할 경우 여기다가 추가하기
    // 우선 달리기가 허기 디버프가 있을 경우 발동하지 못하게 함
    // 기획자분에게 추가로 물어보면 될듯
    public void ActiveSprint()
    {
        if (PlayerStatus.Instance.isStarving || !GameManager.Instance.IsCursorLocked) return;
        movementSpeed *= sprintingSpeed;
        playerInput.isSprinting = true;
        panel.SetActive(true);
    }
    /// <summary>
    /// 플레이어가 달리기를 멈출 경우 발동되는 기능들
    /// </summary>
    // 달릴 때 실행 한것들의 역순으로 적용하기
    public void DeactiveSprint()
    {
        if (!GameManager.Instance.IsCursorLocked) return;
        movementSpeed *= 1f;
        playerInput.isSprinting = false;
        panel.SetActive(false);
    }

    private enum SlopeType
    {
        Plane,
        Walkable,
        Steep
    }

    private SlopeType GetSlopeType()
    {
        // 아랫방향으로 레이를 발사해서 각도가 0이 아닐 경우
        // 0 < 각도 < 캐릭터 각도한계 : Walkable
        // 각도 > 캐릭터 각도한계 : Steep
        // 둘다 아닐경우 Plane을 반환
        if (Physics.Raycast(transform.position, Vector3.down, out var slopeHit, controller.height * 0.5f + slopeRayRange))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle > controller.slopeLimit) return SlopeType.Steep;
            else if (angle < controller.slopeLimit && angle != 0f) return SlopeType.Walkable;
        }
        return SlopeType.Plane;
    }

    private void GetSlopeMovement()
    {
        isSliding = false;
        adjustedMove = worldMoveDir;
        var slope = GetSlopeType();
        // Plane일 경우 : 아무것도 하지 않기
        // Walkable일 경우 : 똑같은 길이로 평면에 투영한 벡터 움직임 구현
        // 추가로 점프가 가능해야 하며 점프는 각도와 상관없이 위로 점프해야 함.
        // Steep일 경우 : slideVelocity의 크기로 아래로 이동, 아래로 슬라이딩 할 경우 점프 불가능.
        switch (slope)
        {
            case SlopeType.Plane:
                break;
            case SlopeType.Walkable:
                adjustedMove = Vector3.ProjectOnPlane(worldMoveDir, Vector3.up).normalized;
                Debug.DrawRay(transform.position, adjustedMove * 2f, Color.red);
                break;
            case SlopeType.Steep:
                isSliding = true;
                if (playerVelocity.y > 0f) return;
                // 빗면 움직임 실행.
                Slide();
                break;
        }
    }

    private void Slide()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var steepHit, slopeRayRange))
        {
            var slideDir = Vector3.ProjectOnPlane(Vector3.down, steepHit.normal).normalized;

            playerVelocity.y += gravity * jumpSpeed * Time.deltaTime;

            var slideVelocity = slideDir * slideSpeed;
            slideVelocity.y = playerVelocity.y;

            controller.Move(slideVelocity * Time.deltaTime);

            Debug.DrawRay(transform.position, slideDir * 2f, Color.red);
        }
    }
}