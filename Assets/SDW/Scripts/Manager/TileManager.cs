using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        var parentTiles = new GameObject();
        parentTiles.name = "Tiles";

        var groundXArea = new Vector2Int(-m_groundTileSize.x / 2, m_groundTileSize.x / 2);
        var groundYArea = new Vector2Int(-m_groundTileSize.y / 2, m_groundTileSize.y / 2);

        for (int y = groundYArea.x; y < groundYArea.y + 1; y++)
        {
            for (int x = groundXArea.x; x < groundXArea.y + 1; x++)
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
                if (x >= groundXArea.x && x <= groundXArea.y && y >= groundYArea.x && y <= groundYArea.y) continue;
                var buildableTile = Instantiate(m_buildableTile);
                buildableTile.transform.position = new Vector3(x, 0, y) * m_tileSize;
                buildableTile.transform.parent = parentTiles.transform;
            }
        }

        var planeTile = Instantiate(m_planeTile);
        planeTile.transform.localScale = new Vector3(m_buildableTileSize.x / 2, 1, m_buildableTileSize.y / 2);
    }
}