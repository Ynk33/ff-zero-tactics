using GravityGoat.ScriptableObject.References;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    // A constant to represent an error cell position
    static Vector3 ERROR_CELL = Vector3.one * 9999f;

    [SerializeField]
    TileVariable hoveredTile;

    [SerializeField]
    TileVariable selectedTile;

    [SerializeField]
    BoolReference isBlocked;

    Camera mainCamera;
    GridManager gridManager;
    TileSelectorCursor cursor;
    Plane worldPlane = new(Vector3.up, Vector3.zero);

    Vector3 mouseWorldPosition = Vector3.zero;

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

        cursor = GetComponentInChildren<TileSelectorCursor>();
        if (cursor == null)
        {
            Debug.LogError("Cursor child not found. Please ensure the TileSelector has a child for the cursor.");
        }
    }

    void Update()
    {
        // If the game is blocked, skip the selection logic
        if (isBlocked.Value)
        {
            hoveredTile.Value = null;
            selectedTile.Value = null;
            cursor.Hide();
            return;
        }

        // Update the mouse world position
        UpdateMousePosition();
        if (mouseWorldPosition == ERROR_CELL)
        {
            cursor.Hide();
            hoveredTile.Value = null;
            return;
        }

        // Update the hovered tile based on the mouse world position
        UpdateHoveredTile();
        if (hoveredTile.Value == null)
        {
            cursor.Hide();
            return;
        }

        // Update the cursor position based on the mouse world position
        cursor.ShowAt(hoveredTile.Value.WorldPosition);

        UpdateSelection();
    }

    void UpdateMousePosition()
    {
        mouseWorldPosition = GetMouseWorldPosition();
        if (mouseWorldPosition == ERROR_CELL)
        {
            return;
        }
    }

    void UpdateHoveredTile()
    {
        if (hoveredTile.Value != null)
        {
            hoveredTile.Value.Hover(false);
        }

        hoveredTile.Value = gridManager.GetTileAt(mouseWorldPosition);

        if (hoveredTile.Value != null)
        {
            hoveredTile.Value.Hover(true);
        }
    }

    void UpdateSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedTile.Value == hoveredTile.Value) return; // Do nothing if the same tile is clicked again

            // Deselect the previously selected tile if there is one
            if (selectedTile.Value != null)
            {
                selectedTile.Value.Select(false);
            }

            // Select the new hovered tile
            selectedTile.Value = hoveredTile.Value;
            selectedTile.Value.Select(true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Deselect the currently selected tile on right-click
            if (selectedTile.Value != null)
            {
                selectedTile.Value.Select(false);
            }

            selectedTile.Value = null;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (worldPlane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return ERROR_CELL; // Return an error cell if no hit is detected
    }
}
