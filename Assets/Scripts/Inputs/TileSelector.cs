using GravityGoat.ScriptableObject.GameEvents;
using GravityGoat.ScriptableObject.References;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    [SerializeField]
    BoolReference isBlocked = default;

    [SerializeField]
    TileGameEvent onTileHovered = default;

    [SerializeField]
    TileGameEvent onTileClicked = default;

    [SerializeField]
    GameEvent onDeselect = default;

    Camera mainCamera;
    GridManager gridManager;
    TileSelectorCursor cursor;
    Plane worldPlane = new(Vector3.up, Vector3.zero);

    bool isActive = false;
    Vector3 mouseWorldPosition = Vector3.zero;

    Tile currentHoveredTile = null;

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
            // Make sure to disable any selection and the cursor if becomes inactive
            if (isActive)
            {
                onDeselect.Raise();
                cursor.Hide();
            }
            isActive = false;
            return;
        }
        else
        {
            isActive = true;
        }

        // Update the mouse world position and ensure it's pointing at the tilemap
        if (!UpdateMousePosition() || !IsWithinBoundaries())
        {
            onTileHovered.Raise(null);
            cursor.Hide();
            return;
        }

        // Update the hovered tile based on the mouse world position
        UpdateHoveredTile();
        // Hide the cursor if not hovering any tile
        if (currentHoveredTile == null)
        {
            cursor.Hide();
            return;
        }

        // Update the cursor position based on the mouse world position
        cursor.ShowAt(currentHoveredTile.WorldPosition);

        UpdateSelection();
    }

    bool UpdateMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (worldPlane.Raycast(ray, out float enter))
        {
            mouseWorldPosition = ray.GetPoint(enter);
            return true;
        }

        return false;
    }

    bool IsWithinBoundaries()
    {
        return gridManager.GetTileAt(mouseWorldPosition) != null;
    }

    void UpdateHoveredTile()
    {
        Tile hoveredTile = gridManager.GetTileAt(mouseWorldPosition);
        if (hoveredTile != currentHoveredTile)
        {
            currentHoveredTile = hoveredTile;
            onTileHovered.Raise(currentHoveredTile);
        }
    }

    void UpdateSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onTileClicked.Raise(currentHoveredTile);
        }
        // Deselect the currently selected tile on right-click
        else if (Input.GetMouseButtonDown(1))
        {
            onDeselect.Raise();
        }
    }
}
