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

    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

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

    private void Update()
    {
        // temp가 없으면 실행하지 않음
        if (!temp)
        {
            return;
        }

        // 마우스 왼쪽 버튼이 눌렸을 경우
        if (Input.GetMouseButtonDown(0))
        {
            // 클릭한 위치가 UI 요소 위인지 확인 (UI 위면 조작 무시)
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            // temp 오브젝트가 아직 배치되지 않았을 경우에만 위치 조정
            if (!temp.Placed)
            {
                // 마우스 클릭 위치를 월드 좌표계로 변환
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // 월드 좌표를 셀 좌표로 변환
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                // 이전 위치와 다를 경우에만 위치 업데이트
                if (prevPos != cellPos)
                {
                    // 오브젝트가 셀의 중앙에 오도록 위치 조정
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                    // 현재 위치를 이전 위치로 저장
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }
    }

    #endregion

        #region Buildilng Placement

    public void InitializeWithBuilding(GameObject building)
    {
        /* 건물 프리팹을 (0, 0, 0) 위치에 회전 없이 생성하고
        생성된 오브젝트에서 Building 컴포넌트를 가져와 temp 변수에 저장 */
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        FollowBuilding();
    }

    private void ClearArea()
    {
        // 이전 영역 크기만큼 타일 배열 생성
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        // 모든 타일을 빈 타일로 채움
        FillTiles(toClear, TileType.Empty);
        // 이전 영역에 빈 타일 설정
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        // 이전 영역 초기화
        ClearArea();

        // 현재 오브젝트 위치를 셀 기준 좌표로 변환하여 영역 위치 설정
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        // 건물이 위치할 영역의 기존 메인 타일맵 데이터 가져오기
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        // 새 타일 배열 생성
        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        // 건물이 놓일 수 있는 위치인지 확인
        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                // 흰 타일 위면 초록색으로 표시 (건물 설치 가능)
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                // 하나라도 조건 불충족 시 전체 영역을 빨간색으로 표시 (건물 설치 불가)
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        // 임시 타일맵에 상태 표시 타일 적용
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        // 현재 영역을 prevArea에 저장하여 다음에 초기화 가능하도록 함
        prevArea = buildingArea;
    }

    #endregion

}
// 타일 타입 지정
public enum TileType
{
Empty, White, Green, Red
}