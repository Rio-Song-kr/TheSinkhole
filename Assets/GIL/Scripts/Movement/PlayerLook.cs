using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera Cam;
    private float xRotation = 0f;
    public float xSens = 30f;
    public float ySens = 30f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// 플레이어 마우스 움직임을 제어
    /// 정면 기준 -80도, 80도의 상하 각도 제한을 두어 목이 꺾이는 듯한 모습을 방지
    /// </summary>
    /// <param name="input"></param>
    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        Cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSens);
    }
}
