using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;

    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();


    #region Unity Methods
    
    // 지정된 영역의 타일 정보를 배열로 가져오는 함수
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        // 영역 크기만큼의 배열 생성
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        // 영역 내 모든 좌표를 순회
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, v.z);
            // 해당 위치의 타일을 배열에 저장
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    // 타일 배열을 특정 타일 타입으로 채우는 함수
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            // 배열의 모든 요소를 지정된 타입의 타일로 설정
            arr[i] = tileBases[type];
        }
    }

    // 지정된 영역에 특정 타입의 타일을 일괄적으로 설정하는 함수
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        // 영역 크기만큼의 타일 배열 생성
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];

        // 배열을 원하는 타일로 채움
        FillTiles(tileArray, type);

        // 타일맵에 해당 영역을 한 번에 설정
        tilemap.SetTilesBlock(area, tileArray);
    }

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        string tilePath = @"Tiles\";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "Tile"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "YesTile"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "NoTile"));

    }

    #endregion

}
// 타일 타입 지정
public enum TileType
{
    Empty, White, Green, Red
}