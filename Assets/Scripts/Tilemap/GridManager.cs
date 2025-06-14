using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static Vector3Int NO_CELL = new(int.MinValue, int.MinValue, int.MinValue);

    [SerializeField]
    Vector3 tileSize = new(1f, 1f, 0f);

    Grid grid;
    Tilemap tilemap;
    Dictionary<Vector3Int, Tile> tiles = new();

    AStar aStar;

    public Tilemap Tilemap => tilemap;

    void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Grid not found in the scene. Please ensure a Grid component is present.");
        }

        tilemap = FindAnyObjectByType<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found in the scene. Please ensure a Tilemap component is present.");
        }

        aStar = new AStar(this);
        FetchTiles();
    }

    public Tile GetTileAt(Vector3Int cell)
    {
        if (tiles.TryGetValue(cell, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Tile GetTileAt(Vector3 worldPosition)
    {
        Vector3Int cell = grid.WorldToCell(worldPosition);
        return GetTileAt(cell);
    }

    public Vector3 GetCellCenterWorldPosition(Vector3Int cell)
    {
        return grid.CellToWorld(cell) + tileSize * 0.5f;
    }

    public void HighlightCellsWithinRange(Vector3 worldPosition, int range, AStar.HeuristicDelegate heuristicFunction)
    {
        Vector3Int centerCell = grid.WorldToCell(worldPosition);
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int cell = new(centerCell.x + x, centerCell.y + y, centerCell.z);
                if (FindPath(centerCell, cell, range, heuristicFunction).Count > 0)
                {
                    if (TryGetFromTile<Tile>(cell, out var tile))
                    {
                        // Show overlay on the tile
                        tile.Highlight(true);
                    }
                }
            }
        }
    }

    public void RemoveHighlights()
    {
        foreach (var tile in tiles.Values)
        {
            tile.Highlight(false);
        }
    }

    public void RemovePath()
    {
        foreach (var tile in tiles.Values)
        {
            tile.TogglePath(false);
        }
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end, int maxDistance, AStar.HeuristicDelegate heuristicFunction)
    {
        return aStar.FindPath(start, end, maxDistance, heuristicFunction);
    }

    public List<Vector3Int> FindPath(Vector3 start, Vector3 end, int maxDistance, AStar.HeuristicDelegate heuristicFunction)
    {
        return aStar.FindPath(grid.WorldToCell(start), grid.WorldToCell(end), maxDistance, heuristicFunction);
    }

    public bool IsWalkable(Vector3Int cell)
    {
        TileBase tile = tilemap.GetTile(cell);
        if (tile == null)
        {
            return false;
        }

        // Check if the tile has a specific property or tag to determine walkability
        return ((TileData)tilemap.GetTile(cell)).walkable;
    }

    public bool IsOccupied(Vector3Int cell)
    {
        return GetTileAt(cell).TileObject != null;
    }

    void FetchTiles()
    {
        tiles.Clear();
        foreach (var tile in tilemap.GetComponentsInChildren<Tile>())
        {
            Vector3Int cell = tilemap.WorldToCell(tile.transform.position);
            tiles[cell] = tile;
        }
    }

    bool TryGetFromTile<T>(Vector3Int cell, out T component) where T : Component
    {
        component = null;
        GameObject tileObject = tilemap.GetInstantiatedObject(cell);
        if (tileObject != null)
        {
            component = tileObject.GetComponent<T>();
            return true;
        }

        return false;
    }
}
