using UnityEngine;

public class TileObject : MonoBehaviour
{
    GridManager gridManager;
    Tile tile;
    Selectable selectable;

    public Vector3 Position => transform.position;

    protected GridManager GridManager => gridManager;

    protected virtual void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridObjectMap not found in the scene. Please ensure a GridObjectMap component is present.");
        }

        selectable = GetComponent<Selectable>();
    }

    protected virtual void Start()
    {
        UpdateTile();
    }

    public void Hover(bool isHovered)
    {
        if (selectable == null)
        {
            return;
        }

        OnHover(isHovered);
    }

    public void Select(bool isSelected)
    {
        if (selectable == null)
        {
            return;
        }

        OnSelect(isSelected);
    }

    protected virtual void OnHover(bool isHovered)
    {
        selectable.Hover(isHovered);
    }

    protected virtual void OnSelect(bool isSelected)
    {
        selectable.Select(isSelected);
    }

    protected virtual void UpdateTile()
    {
        // Unset the previous tile object reference
        if (tile != null)
        {
            tile.TileObject = null;
        }

        // Get the new tile at the current position and set the tile object reference
        tile = gridManager.GetTileAt(transform.position);
        if (tile != null)
        {
            tile.TileObject = this;
        }
    }
}
