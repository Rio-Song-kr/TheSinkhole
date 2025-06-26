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

    #region Unity Methods

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
                }
            }
        }
    }

    #endregion

        #region Buildilng Placement

    public void InitializeWithBuilding(GameObject building)
    {
        /* �ǹ� �������� (0, 0, 0) ��ġ�� ȸ�� ���� �����ϰ�
        ������ ������Ʈ���� Building ������Ʈ�� ������ temp ������ ���� */
        temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<Building>();
    }

#endregion

}
// Ÿ�� Ÿ�� ����
public enum TileType
{
Empty, White, Green, Red
}