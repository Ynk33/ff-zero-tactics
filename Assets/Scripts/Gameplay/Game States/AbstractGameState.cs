using UnityEngine;

public abstract class AbstractGameState : ScriptableObject
{
    [SerializeField]
    GridManagerReference gridManager = default;

    GameStateManager gameStateManager;

    protected GameStateManager GameStateManager => gameStateManager;
    protected GridManager GridManager => gridManager.Value;

    public void Init(GameStateManager gameStateManager, Tile hoveredTile, Tile selectedTile)
    {
        this.gameStateManager = gameStateManager;
        OnTileHovered(hoveredTile);
        OnTileSelected(selectedTile);
    }

    public abstract void OnTileHovered(Tile hoveredTile);
    public abstract void OnTileSelected(Tile selectedTile);
    public abstract void OnDeselect();
    public abstract void OnExit();
}