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
    [Header("Test")]
    public TextMeshProUGUI curMoveVelocityText;
    public GameObject panel;
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
    /// <summary>
    /// 플레이어가 달릴 경우 발동되는 기능들
    /// </summary>
    // 추후에 이펙트 or UI 반응을 추가할 경우 여기다가 추가하기
    // 우선 달리기가 허기 디버프가 있을 경우 발동하지 못하게 함
    // 기획자분에게 추가로 물어보면 될듯
    public void ActiveSprint()
    {
        if (PlayerStatus.Instance.isStarving) return;
        speed *= sprintingSpeed;
        playerInput.isSprinting = true;
        panel.SetActive(true);
    }
    /// <summary>
    /// 플레이어가 달리기를 멈출 경우 발동되는 기능들
    /// </summary>
    // 달릴 때 실행 한것들의 역순으로 적용하기
    public void DeactiveSprint()
    {
        speed *= 1f;
        playerInput.isSprinting = false;
        panel.SetActive(false);
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
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        Vector3 worldMoveDir = transform.TransformDirection(moveDir);
        float currentSpeed = isSprinting && !PlayerStatus.Instance.isStarving ? speed * sprintingSpeed : speed;
        Vector3 moveVelocity = worldMoveDir * currentSpeed;

        // TODO : [Test] 추후에 개발이 완성되면 지울 것!
        #region [Test]
        float horizontalSpeed = new Vector3(moveVelocity.x, 0f, moveVelocity.z).magnitude;
        if (curMoveVelocityText != null) curMoveVelocityText.text = $"Speed: {horizontalSpeed:F2}";
        #endregion
        
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
