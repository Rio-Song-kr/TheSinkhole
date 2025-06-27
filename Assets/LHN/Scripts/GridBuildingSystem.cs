using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public TileType type;

    public void Build()
    {
        // Ÿ���� White��� ��ġ ����
        if (type == TileType.White)
        {
            //ok
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
}

// Ÿ�� Ÿ�� ����
public enum TileType
{
    White, Green, Red
}