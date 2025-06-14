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

    AStar aStar;
    List<GridObject> gridObjects = new();
    Selectable selectedObject = null;
    Unit selectedUnit = null;
    Vector3Int hoveredCell = NO_CELL;
    bool isBusy = false;

    public Grid Grid => grid;
    public Tilemap Tilemap => tilemap;
    public Vector3Int HoveredCell => hoveredCell;
    public bool IsBusy => isBusy;

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
        hoveredCell = grid.WorldToCell(worldPosition);

        // Highlight the tile under the mouse cursor
        if (TryGetFromTile<Highlightable>(hoveredCell, out var highlightableTile))
        {
            highlightableTile.Highlight();
        }

        // Highlight object in the tile
        GameObject obj = GetObjectAt(hoveredCell);
        if (obj != null)
        {
            if (obj.TryGetComponent<Highlightable>(out var highlightableObject))
            {
                highlightableObject.Highlight();
            }
        }
    }

    public void ShowCellsWithinRange(Vector3 worldPosition, int range)
    {
        Vector3Int centerCell = grid.WorldToCell(worldPosition);
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int cell = new(centerCell.x + x, centerCell.y + y, centerCell.z);
                if (FindPath(centerCell, cell, range).Count > 0)
                {
                    if (TryGetFromTile<Tile>(cell, out var tile))
                    {
                        // Show overlay on the tile
                        tile.ShowOverlay();
                    }
                }
            }
        }
    }

    public void HideAllHighlights()
    {
        hoveredCell = NO_CELL; // Reset hovered cell
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
    public Vector3 GetCellCenterWorldPosition(Vector3Int cell)
    {
        return grid.CellToWorld(cell) + tileSize * 0.5f;
    }

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

    public void ShowArrowAt(Vector3Int cell)
    {
        if (TryGetFromTile<Tile>(cell, out var tile))
        {
            tile.ShowArrow();
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

    public void SelectAt(Vector3 worldPosition)
    {
        Vector3Int cell = grid.WorldToCell(worldPosition);

        GameObject obj = GetObjectAt(cell);
        // If an object is found at the cell, select it
        if (obj != null)
        {
            Deselect();

            Selectable selectable = obj.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectedObject = selectable;
                selectedUnit = selectedObject.GetComponent<Unit>();
                selectedObject.Select();
            }
        }
        // If no object is found but an object is already selected, move it to the selected cell and deselect it
        else if (selectedObject != null && selectedObject.TryGetComponent<GridObject>(out var gridObject))
        {
            if (gridObject.TryGetComponent<Unit>(out var unit))
            {
                isBusy = true;
                unit.MoveTo(cell, () =>
                {
                    isBusy = false;
                    selectedUnit = null;
                });
            }

            Deselect();
        }
        // If no object is found and no object is selected, deselect any currently selected object
        else
        {
            Deselect();
        }
    }

    public void Deselect()
    {
        if (selectedObject != null)
        {
            selectedObject.Deselect();
        }
        selectedObject = null;
    }
    #endregion

    #region Pathfinding
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end, int maxDistance)
    {
        return aStar.FindPath(start, end, maxDistance);
    }

    public List<Vector3Int> FindPath(Vector3 start, Vector3Int end, int maxDistance)
    {
        return aStar.FindPath(grid.WorldToCell(start), end, maxDistance);
    }
    
    public float Heuristic(Vector3Int a, Vector3Int b)
    {
        return selectedUnit.GetCost(a, b);
    }
    #endregion

}
