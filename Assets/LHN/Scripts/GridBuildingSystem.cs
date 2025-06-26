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
        // temp�� ������ �������� ����
        if (!temp)
        {
            return;
        }

        // ���콺 ���� ��ư�� ������ ���
        if (Input.GetMouseButtonDown(0))
        {
            // Ŭ���� ��ġ�� UI ��� ������ Ȯ�� (UI ���� ���� ����)
            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return;
            }

            // temp ������Ʈ�� ���� ��ġ���� �ʾ��� ��쿡�� ��ġ ����
            if (!temp.Placed)
            {
                // ���콺 Ŭ�� ��ġ�� ���� ��ǥ��� ��ȯ
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // ���� ��ǥ�� �� ��ǥ�� ��ȯ
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                // ���� ��ġ�� �ٸ� ��쿡�� ��ġ ������Ʈ
                if (prevPos != cellPos)
                {
                    // ������Ʈ�� ���� �߾ӿ� ������ ��ġ ����
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3(.5f, .5f, 0f));
                    // ���� ��ġ�� ���� ��ġ�� ����
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }
        // �����̽��ٸ� ������ ���
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // �ӽ� ������Ʈ�� �ش� ��ġ�� ��ġ �����ϸ�
            if (temp.CanBePlaced())
            {
                // ������Ʈ�� ��ġ
                temp.Place();
            }
        }
        // ESC Ű�� ������ ���
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // �ӽ� ��ġ ���� �ʱ�ȭ (Ÿ�� ���� ��)
            ClearArea();

            // �ӽ� ������Ʈ ����
            Destroy(temp.gameObject);
        }

    }

    #endregion

    #region Tilemap Management

    // ������ ������ Ÿ�� ������ �迭�� �������� �Լ�
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        // ���� ũ�⸸ŭ�� �迭 ����
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        // ���� �� ��� ��ǥ�� ��ȸ
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, v.z);
            // �ش� ��ġ�� Ÿ���� �迭�� ����
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    // Ÿ�� �迭�� Ư�� Ÿ�� Ÿ������ ä��� �Լ�
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            // �迭�� ��� ��Ҹ� ������ Ÿ���� Ÿ�Ϸ� ����
            arr[i] = tileBases[type];
        }
    }

    // ������ ������ Ư�� Ÿ���� Ÿ���� �ϰ������� �����ϴ� �Լ�
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        // ���� ũ�⸸ŭ�� Ÿ�� �迭 ����
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];

        // �迭�� ���ϴ� Ÿ�Ϸ� ä��
        FillTiles(tileArray, type);

        // Ÿ�ϸʿ� �ش� ������ �� ���� ����
        tilemap.SetTilesBlock(area, tileArray);
    }

    #endregion

    #region Buildilng Placement

    // ���޹��� �ǹ� �������� ������� �ӽ� Building ��ü�� �����ϴ� �Լ�
    public void InitializeWithBuilding(GameObject building)
    {
        /* �ǹ� �������� (0, 0, 0) ��ġ�� ȸ�� ���� �����ϰ�
        ������ ������Ʈ���� Building ������Ʈ�� ������ temp ������ ���� */
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
        FollowBuilding();
    }

    // ������ ǥ�õ� �ӽ� ������ �ʱ�ȭ (�� Ÿ�Ϸ� ����)
    private void ClearArea()
    {
        // ���� ���� ũ�⸸ŭ Ÿ�� �迭 ����
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        // ��� Ÿ���� �� Ÿ�Ϸ� ä��
        FillTiles(toClear, TileType.Empty);
        // ���� ������ �� Ÿ�� ����
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        // ���� ���� ���� �ʱ�ȭ
        ClearArea();

        // ���� ������Ʈ ��ġ�� �� ���� ��ǥ�� ��ȯ�Ͽ� ���� ��ġ ����
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        // �ǹ��� ��ġ�� ������ ���� ���� Ÿ�ϸ� ������ ��������
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        // �� Ÿ�� �迭 ����
        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        // �ǹ��� ���� �� �ִ� ��ġ���� Ȯ��
        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                // �� Ÿ�� ���� �ʷϻ����� ǥ�� (�ǹ� ��ġ ����)
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                // �ϳ��� ���� ������ �� ��ü ������ ���������� ǥ�� (�ǹ� ��ġ �Ұ�)
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }
        // �ӽ� Ÿ�ϸʿ� ���� ǥ�� Ÿ�� ����
        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        // ���� ������ prevArea�� �����Ͽ� ������ �ʱ�ȭ �����ϵ��� ��
        prevArea = buildingArea;
    }

    // �־��� ������ �ǹ� ��ġ �������� Ȯ��
    public bool CanTakeArea(BoundsInt area)
    {
        // MainTilemap���� ������ ����(area)�� ��� Ÿ���� ������
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);

        // �� Ÿ���� �ϳ��� �˻�
        foreach (var b in baseArray)
        {
            // ��� Ÿ��(��ġ ������ Ÿ��)�� �ƴ� ��찡 �ϳ��� ������
            if (b != tileBases[TileType.White])
            {
                // ��ġ �Ұ� ��� �� false ��ȯ
                Debug.Log("��ġ �Ұ�");
                return false;
            }
        }

        // ��� Ÿ���� ����̸� ��ġ �����ϹǷ� true ��ȯ
        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
}
#endregion


// Ÿ�� Ÿ�� ����
public enum TileType
{
    Empty, White, Green, Red
}