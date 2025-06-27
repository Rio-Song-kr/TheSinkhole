using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Building buildingPrefab;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ������ ��ġ�� ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                // �浹�� ������ ������Ʈ�� tile �� ����
                GridBuildingSystem tile = hitInfo.collider.GetComponent<GridBuildingSystem>();

                // Ÿ���� Ÿ���� White��� ���� ����
                if (tile.type == TileType.White)
                {
                    tile.Build(buildingPrefab);
                }
                else
                {
                    Debug.Log("�̹� ��ġ �Ǿ�����");
                }
            }
        }
    }
}
