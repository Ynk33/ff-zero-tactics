using UnityEngine;

[CreateAssetMenu(fileName = "Default Game State", menuName = "Scriptable Objects/Game States/Default Game State")]
public class DefaultGameState : AbstractGameState
{
    [SerializeField]
    AbstractGameState stateOnUnitSelected = default;

    Unit hoveredUnit = null;

    public override void OnTileHovered(Tile hoveredTile)
    {
        if (hoveredTile == null) return;

        // Fetch the unit on the hovered tile
        Unit unit = hoveredTile.TileObject as Unit;

        // Hide the movement range of the previously hovered unit, if any
        if (hoveredUnit != null)
        {
            hoveredUnit.HideMovementRange();
        }

        if (unit == null) return; // No unit hovered, nothing else to do

        // Show movement range of the currently hovered unit
        hoveredUnit = unit;
        hoveredUnit.ShowMovementRange();
    }

    public override void OnTileSelected(Tile selectedTile)
    {
        if (selectedTile == null) return;

        // Fetch the unit on the selected tile
        Unit unit = selectedTile.TileObject as Unit;

        // If an unit is selected, switch state to movement state
        if (unit != null)
        {
            GameStateManager.SwitchState(stateOnUnitSelected);
        }
    }

    public override void OnDeselect() {}

    public override void OnExit()
    {
        if (hoveredUnit != null) hoveredUnit.HideMovementRange();
    }
}