using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    // A constant to represent an error cell position
    static Vector3 ERROR_CELL = Vector3.one * 9999f;

    Camera mainCamera;
    GridManager gridManager;

    void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Please ensure a Camera with the 'MainCamera' tag is present in the scene.");
        }

        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure a GridManager component is present.");
        }
    }

    void LateUpdate()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        if (mouseWorldPosition == ERROR_CELL)
        {
            gridManager.HideAllHighlights();
            return;
        }

        // Highlight the cell under the mouse cursor
        gridManager.HighlightCellAt(mouseWorldPosition);

        if (Input.GetMouseButtonDown(0))
        {
            gridManager.Deselect();
            gridManager.SelectObjectAt(mouseWorldPosition);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            gridManager.Deselect();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return ERROR_CELL; // Return an error cell if no hit is detected
    }
}
