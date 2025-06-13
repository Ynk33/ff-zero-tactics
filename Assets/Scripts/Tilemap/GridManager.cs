using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    Grid grid;
    Tilemap tilemap;

    public Grid Grid => grid;
    public Tilemap Tilemap => tilemap;

    AStar aStar;
    List<GridObject> gridObjects = new();
    Selectable selectedObject = null;

    void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
        tilemap = FindAnyObjectByType<Tilemap>();

        aStar = new AStar(this);
    }

    void Start()
    {
        if (grid == null)
        {
            Debug.LogError("Grid not found in the scene. Please ensure a Grid component is present.");
        }
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found in the scene. Please ensure a Tilemap component is present.");
        }
    }

    #region Grid Operations
    public void HighlightCellAt(Vector3 worldPosition)
    {
        Vector3Int cell = grid.WorldToCell(worldPosition);

        // Highlight the tile under the mouse cursor
        tilemap.GetInstantiatedObject(cell)?.GetComponent<Highlightable>()?.Highlight();

        // Highlight object in the tile
        GameObject obj = GetObjectAt(cell);
        if (obj != null)
        {
            obj.GetComponent<Highlightable>()?.Highlight();
        }
    }

    public void HighlightCellsWithinRange(Vector3 worldPosition, int range)
    {
        Vector3Int centerCell = grid.WorldToCell(worldPosition);
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int cell = new Vector3Int(centerCell.x + x, centerCell.y + y, centerCell.z);
                if (aStar.FindPath(centerCell, cell, range).Count > 0)
                {
                    tilemap.GetInstantiatedObject(cell)?.GetComponent<Tile>()?.ShowOverlay();
                }
            }
        }
    }
    
    public void HideAllOverlays()
    {
        foreach (var tile in tilemap.GetComponentsInChildren<Tile>())
        {
            tile.HideOverlay();
        }
    }
    #endregion

    #region Tile Management
    public bool IsWalkable(Vector3Int cell)
    {
        TileBase tile = tilemap.GetTile(cell);
        if (tile == null)
        {
            return true; // No tile means it's walkable
        }

        // Check if the tile has a specific property or tag to determine walkability
        return ((TileData)tilemap.GetTile(cell)).walkable;
    }
    #endregion

    #region Object Management
    public void AddObject(GridObject gridObject)
    {
        if (!gridObjects.Contains(gridObject))
        {
            gridObjects.Add(gridObject);
        }
        else
        {
            Debug.LogWarning("GridObject is already in the map: " + gridObject.name);
        }
    }

    public GameObject GetObjectAt(Vector3Int cellPosition)
    {
        foreach (var gridObject in gridObjects)
        {
            Vector3Int objectCellPosition = grid.WorldToCell(gridObject.transform.position);
            if (objectCellPosition == cellPosition)
            {
                return gridObject.gameObject;
            }
        }

        return null;
    }

    public bool IsOccupied(Vector3Int cell)
    {
        return GetObjectAt(cell) != null;
    }

    public void SelectObjectAt(Vector3 worldPosition)
    {
        Vector3Int cell = grid.WorldToCell(worldPosition);

        GameObject obj = GetObjectAt(cell);
        if (obj != null)
        {
            Selectable selectable = obj.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectable.Select();
                selectedObject = selectable;
            }
        }
    }

    public void Deselect()
    {
        selectedObject?.Deselect();
        selectedObject = null;
    }
    #endregion
}
