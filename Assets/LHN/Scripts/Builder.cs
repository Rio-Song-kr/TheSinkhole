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
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                // �浹�� ������ ������Ʈ�� tile �� ����
                var tile = hitInfo.collider.GetComponent<GridBuildingSystem>();

                if (tile == null) return;

                // Ÿ���� Ÿ���� White��� ���� ����
                if (tile.type == TileType.White)
                {
                    tile.Build(buildingPrefab);
                }
                else if (tile.type == TileType.Green)
                {
                    tile.UnBuild(buildingPrefab);
                }
            }
        }
    }
}