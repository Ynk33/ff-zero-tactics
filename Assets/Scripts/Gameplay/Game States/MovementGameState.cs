using System.Collections.Generic;
using GravityGoat.ScriptableObject.References;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Game State", menuName = "Scriptable Objects/Game States/Movement Game State")]
public class MovementGameState : AbstractGameState
{
    [SerializeField]
    BoolReference isBlocking = default;

    [SerializeField]
    float movementDurationPerCell = 0.1f;

    Unit selectedUnit;

    public override void OnTileHovered(Tile hoveredTile)
    {
        GridManager.RemovePath();

        if (hoveredTile == null) return;

        if (selectedUnit != null)
        {
            // If within range
            List<Vector3Int> path = selectedUnit.FindPathTo(hoveredTile.WorldPosition);
            if (path.Count > 0)
            {
                foreach (var point in path)
                {
                    Tile tile = GridManager.GetTileAt(point);
                    if (tile == null) continue;

                    tile.TogglePath(true);
                }
            }
        }
    }

    public override void OnTileSelected(Tile selectedTile)
    {
        if (selectedTile == null) return;

        // Fetch the unit on the selected tile
        Unit unit = selectedTile.TileObject as Unit;

        // If an unit is already selected...
        if (selectedUnit != null)
        {
            // ...and the selected tile is another unit...
            if (unit != null)
            {
                // Update selection
                selectedUnit.Select(false);
                selectedUnit.HideMovementRange();

                selectedUnit = unit;

                selectedUnit.Select(true);
                selectedUnit.ShowMovementRange();
            }
            // ...and the selected tile is empty...
            else
            {
                // If the tile is within range
                List<Vector3Int> path = selectedUnit.FindPathTo(selectedTile.WorldPosition);
                if (path.Count > 0)
                {
                    // TODO: unit.Move();
                    isBlocking.Value = true;
                    selectedUnit.MoveTo(selectedTile.WorldPosition, movementDurationPerCell, () =>
                    {
                        isBlocking.Value = false;
                    });
                    GameStateManager.ToDefault();
                }
                else
                {
                    // Click happened outside of the unit range => back to game's default state
                    GameStateManager.ToDefault();
                }
            }
        }
        // Else if no unit selected...
        else
        {
            // ...and the selected tile is an unit...
            if (unit != null)
            {
                // Update the selection
                selectedUnit = unit;

                selectedUnit.Select(true);
                selectedUnit.ShowMovementRange();
            }
            else
            {
                // Click on an empty tile and no unit currently selected => back to game's default state
                GameStateManager.ToDefault();
            }
        }
    }

    public override void OnDeselect()
    {
        GameStateManager.ToDefault();
    }

    public override void OnExit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.Select(false);
            selectedUnit.HideMovementRange();
        }
    }
}