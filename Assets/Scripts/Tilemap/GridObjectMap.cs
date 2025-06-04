using System.Collections.Generic;
using UnityEngine;

public class GridObjectMap : MonoBehaviour
{
    private Grid grid;

    private List<GridObject> gridObjects = new();

    void Awake()
    {
        grid = FindAnyObjectByType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Grid not found in the scene. Please ensure a Grid component is present.");
        }
    }

    public void Add(GridObject gridObject)
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

    public List<GameObject> GetObjectsInCell(Vector3Int cellPosition)
    {
        List<GameObject> objectsInCell = new List<GameObject>();

        foreach (var gridObject in gridObjects)
        {
            Vector3Int objectCellPosition = grid.WorldToCell(gridObject.transform.position);
            if (objectCellPosition == cellPosition)
            {
                objectsInCell.Add(gridObject.gameObject);
            }
        }

        return objectsInCell;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return grid.CellToWorld(cellPosition);
    }
}