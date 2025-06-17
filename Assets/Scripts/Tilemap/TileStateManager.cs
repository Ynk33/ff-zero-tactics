using UnityEngine;

[CreateAssetMenu(fileName = "Tile State Manager", menuName = "Scriptable Objects/Game States/Tile State Manager")]
public class TileStateManager : AbstractGameState
{
    Tile currentHoveredTile = null;

    public override void OnTileHovered(Tile hoveredTile)
    {
        if (currentHoveredTile != null) currentHoveredTile.Hover(false);

        currentHoveredTile = hoveredTile;

        if (currentHoveredTile != null) currentHoveredTile.Hover(true);
    }

    public override void OnTileSelected(Tile selectedTile) { }

    public override void OnDeselect() { }

    public override void OnExit() {}
}
