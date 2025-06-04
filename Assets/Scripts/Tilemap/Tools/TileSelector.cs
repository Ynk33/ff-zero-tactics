using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    Vector3Int ERROR_CELL = new Vector3Int(-int.MaxValue, -int.MaxValue, -int.MaxValue);

    Camera mainCamera;
    Grid grid;
    Tilemap tilemap;
    GridObjectMap gridObjectMap;

    List<Selectable> selectedObjects = new List<Selectable>();

    void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Please ensure a Camera with the 'MainCamera' tag is present in the scene.");
        }

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

        gridObjectMap = FindAnyObjectByType<GridObjectMap>();
        if (gridObjectMap == null)
        {
            Debug.LogError("GridObjectMap not found in the scene. Please ensure a GridObjectMap component is present.");
        }
    }

    void LateUpdate()
    {
        Vector3Int hoveredCell = GetHoveredCell();
        Hover(hoveredCell);

        if (Input.GetMouseButtonDown(0))
        {
            Select(hoveredCell);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    void Hover(Vector3Int cell)
    {
        if (cell != ERROR_CELL)
        {
            // Highlight the tile under the mouse cursor
            tilemap.GetInstantiatedObject(cell)?.GetComponent<Highlightable>()?.Highlight();

            // Highlight all objects in the tile
            List<GameObject> objectsInCell = gridObjectMap.GetObjectsInCell(cell);
            foreach (GameObject obj in objectsInCell)
            {
                obj.GetComponent<Highlightable>()?.Highlight();
            }
        }
    }

    void Select(Vector3Int cell)
    {
        Deselect();
        
        List<GameObject> objectsInCell = gridObjectMap.GetObjectsInCell(cell);
        foreach (GameObject obj in objectsInCell)
        {
            Selectable selectable = obj.GetComponent<Selectable>();
            if (selectable != null && !selectedObjects.Contains(selectable))
            {
                selectable.Select();
                selectedObjects.Add(selectable);
            }
        }
    }

    void Deselect()
    {
        foreach (Selectable selectable in selectedObjects)
        {
            selectable.Deselect();
        }

        selectedObjects.Clear();
    }

    Vector3Int GetHoveredCell()
    {
        // This method can be used to get the currently hovered tile if needed
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3Int cell = grid.WorldToCell(hit.point);
            return cell;
        }

        return ERROR_CELL; // Return an error cell if no hit is detected
    }   
}
