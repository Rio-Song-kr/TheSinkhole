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

    // ���콺 ��ġ�� ������� �� ���� ���õ� ��ġ�� ����ϴ� �ڵ�
    public Vector3 GetSelectionMapPosition()
    {
        // ���� ���콺�� ȭ�� ��ǥ�� ������
        Vector3 mousePos = Input.mousePosition;

        // ȭ�� ��ǥ�� �������� ī�޶󿡼� ����(ray)�� ����
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        // ���̸� ���� ��Ƽ� �浹�� ������Ʈ�� �ִ��� Ȯ��
        if (Physics.Raycast(ray, out hit))
        {
            // ���콺�� �ִ� ��ǥ�� �ַܼ� ���
            Debug.Log(hit.point);
            // ���������� ���õ� ��ġ�� ����
            lastPosition = hit.point;
        }

        // ���������� ����� ��ġ�� ��ȯ
        return lastPosition;
    }

}
