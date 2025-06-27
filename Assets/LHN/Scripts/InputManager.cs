using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField] private LayerMask placementLayermask;

    private void Update()
    {
        GetSelectionMapPosition();
    }

    // 마우스 위치를 기반으로 맵 상의 선택된 위치를 출력하는 코드
    public Vector3 GetSelectionMapPosition()
    {
        // 현재 마우스의 화면 좌표를 가져옴
        Vector3 mousePos = Input.mousePosition;

        // 화면 좌표를 기준으로 카메라에서 레이(ray)를 생성
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        // 레이를 씬에 쏘아서 충돌한 오브젝트가 있는지 확인
        if (Physics.Raycast(ray, out hit))
        {
            // 마우스가 있는 좌표를 콘솔로 출력
            Debug.Log(hit.point);
            // 마지막으로 선택된 위치를 저장
            lastPosition = hit.point;
        }

        // 마지막으로 저장된 위치를 반환
        return lastPosition;
    }

}
