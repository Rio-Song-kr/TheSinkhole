using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public TileType type;
    public Building building;

    public void Build(Building buildingPrefab)
    {
        // Ÿ���� White��� ��ġ ����
        if (type == TileType.White)
        {
            Vector3 buildingPos = transform.position + Vector3.up * 0.01f;
            building = Instantiate(buildingPrefab, buildingPos, Quaternion.identity);
            type = TileType.Green;
        }
        // Ÿ���� green�̶�� ��ġ �Ұ���
        else if (type == TileType.Green)
        {
            //no
        }
        // Ÿ���� Red��� ��ġ �Ұ���
        else if (type == TileType.Red)
        {
            //no
        }
    }

    public void UnBuild(Building buildingPrefab)
    {
        // Ÿ���� White��� ���� �Ұ���
        if (type == TileType.White)
        {
            //no
        }
        // Ÿ���� green�̶�� ���� ����
        else if (type == TileType.Green)
        {
            Destroy(building.gameObject);
            type = TileType.White;
        }
        // Ÿ���� Red��� ���� �Ұ���
        else if (type == TileType.Red)
        {
            //no
        }
    }
}

// Ÿ�� Ÿ�� ����
public enum TileType
{
    White, Green, Red
}