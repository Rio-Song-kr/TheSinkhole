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
        // 타일이 White라면 설치 가능
        if (type == TileType.White)
        {
            Vector3 buildingPos = transform.position + Vector3.up * 0.01f;
            building = Instantiate(buildingPrefab, buildingPos, Quaternion.identity);
            type = TileType.Green;
        }
        // 타일이 green이라면 설치 불가능
        else if (type == TileType.Green)
        {
            //no
        }
        // 타일이 Red라면 설치 불가능
        else if (type == TileType.Red)
        {
            //no
        }
    }

    public void UnBuild(Building buildingPrefab)
    {
        // 타일이 White라면 제거 불가능
        if (type == TileType.White)
        {
            //no
        }
        // 타일이 green이라면 제거 가능
        else if (type == TileType.Green)
        {
            Destroy(building.gameObject);
            type = TileType.White;
        }
        // 타일이 Red라면 제거 불가능
        else if (type == TileType.Red)
        {
            //no
        }
    }
}

// 타일 타입 지정
public enum TileType
{
    White, Green, Red
}