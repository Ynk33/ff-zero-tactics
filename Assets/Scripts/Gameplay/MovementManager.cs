using System.Collections.Generic;
using GravityGoat.ScriptableObject.Variables;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    TileReference hoveredTile;

    [SerializeField]
    TileReference selectedTile;

    [SerializeField]
    BoolVariable isBlocking;

    [SerializeField]
    float movementDuration = 0.1f;

    GridManager gridManager;
    Unit selectedUnit = null;

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure a GridManager component is present.");
        }
    }

    public void OnTileSelectedChanged()
    {
        gridManager.RemoveHighlights();

        // If no selected tile, reset variables and do nothing.
        if (selectedTile.Value == null)
        {
            selectedUnit = null;
            return;
        }

        Unit selectedTileUnit = selectedTile.Value.TileObject as Unit;
        // If selected tile has an unit, switch selection to this unit and show its movement possibilities.
        if (selectedTileUnit != null)
        {
            selectedUnit = selectedTileUnit;
            ShowMovementPossibilities();
        }
        else
        {
            // If the selected tile doesn't have an unit and an unit is already selected...
            if (selectedUnit != null)
            {
                // ... move the unit to the new tile
                isBlocking.Value = true;
                selectedUnit.MoveTo(selectedTile.Value.WorldPosition, movementDuration, () =>
                {
                    selectedUnit = null;
                    isBlocking.Value = false;
                });
            }
        }
    }

    public void OnTileHoveredChanged()
    {
        gridManager.RemovePath();

        if (selectedTile.Value == null || selectedUnit == null) return; // No tile selected, nothing to do.
        if (hoveredTile.Value == null) return; // No tile hovered, nothing to do.

        ShowPath();
    }

    void ShowMovementPossibilities()
    {
        gridManager.HighlightCellsWithinRange(selectedTile.Value.WorldPosition, selectedUnit.MaxDistance, selectedUnit.GetCost);
    }

    void ShowPath()
    {
        List<Vector3Int> path = selectedUnit.FindPathTo(hoveredTile.Value.WorldPosition);
        foreach (Vector3Int cell in path)
        {
            gridManager.GetTileAt(cell).TogglePath(true);
        }
    }
}
