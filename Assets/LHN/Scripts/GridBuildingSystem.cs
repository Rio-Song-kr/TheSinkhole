using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public TileType type;

    public void Build()
    {
        // 타일이 White라면 설치 가능
        if (type == TileType.White)
        {
            //ok
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
}

// 타일 타입 지정
public enum TileType
{
    White, Green, Red
}