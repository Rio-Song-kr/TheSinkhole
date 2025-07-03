using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 기본 맵 Tile 생성을 위한 클래스
/// </summary>
public class TileManager : MonoBehaviour
{
    [Header("Tile Prefabs")]
    [SerializeField] private GameObject m_groundTile;
    [SerializeField] private GameObject m_buildableTile;
    [SerializeField] private GameObject m_planeTile;

    [Header("Map Size")]
    [SerializeField] private Vector2Int m_groundTileSize;
    [SerializeField] private Vector2Int m_buildableTileSize;
    [SerializeField] private int m_tileSize;

    private Vector2Int m_groundXArea;
    private Vector2Int m_groundYArea;

    //todo 울타리 - 출구 주변 3개 block은 개척이 가능한 tile로 변경되어야 함
    private void Awake()
    {
        var parentTiles = new GameObject();
        parentTiles.name = "Tiles";

        m_groundXArea = new Vector2Int(-m_groundTileSize.x / 2, m_groundTileSize.x / 2);
        m_groundYArea = new Vector2Int(-m_groundTileSize.y / 2, m_groundTileSize.y / 2);

        for (int y = m_groundYArea.x; y < m_groundYArea.y + 1; y++)
        {
            for (int x = m_groundXArea.x; x < m_groundXArea.y + 1; x++)
            {
                var groundTile = Instantiate(m_groundTile);
                groundTile.transform.position = new Vector3(x, 0, y) * m_tileSize;
                groundTile.transform.parent = parentTiles.transform;
            }
        }

        for (int y = -m_buildableTileSize.y / 2; y < m_buildableTileSize.y / 2 + 1; y++)
        {
            for (int x = -m_buildableTileSize.x / 2; x < m_buildableTileSize.x / 2 + 1; x++)
            {
                if (x >= m_groundXArea.x && x <= m_groundXArea.y && y >= m_groundYArea.x && y <= m_groundYArea.y) continue;
                var buildableTile = Instantiate(m_buildableTile);
                buildableTile.transform.position = new Vector3(x, 0, y) * m_tileSize;
                buildableTile.transform.parent = parentTiles.transform;

                //# Tile에 미리 Surface를 Build하더라도 Surface의 위치가 변하지 않기에 Runtime에 빌드
                var navMeshSurface = buildableTile.GetComponent<NavMeshSurface>();
                if (navMeshSurface != null) navMeshSurface.BuildNavMesh();

                var links = buildableTile.GetComponents<NavMeshLink>();
                foreach (var link in links)
                {
                    link.UpdateLink();
                }
            }
        }

        var planeTile = Instantiate(m_planeTile);
        planeTile.transform.localScale = new Vector3(m_buildableTileSize.x / 2, 1, m_buildableTileSize.y / 2);
        planeTile.transform.parent = parentTiles.transform;
    }

    public Vector2Int GetFenceXArea() => m_groundXArea * m_tileSize;
    public Vector2Int GetFenceYArea() => m_groundYArea * m_tileSize;
}