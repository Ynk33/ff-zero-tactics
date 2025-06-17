using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    AbstractGameState defaultGameState = default;

    [SerializeField]
    List<AbstractGameState> globalGameStates = default;

    [ShowInInspector, ReadOnly]
    AbstractGameState state = null;
    Tile hoveredTile = null;
    Tile selectedTile = null;

    void Start()
    {
        foreach (var state in globalGameStates)
        {
            state.Init(this, hoveredTile, selectedTile);
        }
        
        ToDefault();
    }

    public void SwitchState(AbstractGameState newState)
    {
        if (state != null) state.OnExit();

        state = newState;
        state.Init(this, hoveredTile, selectedTile);
    }

    public void ToDefault()
    {
        SwitchState(defaultGameState);
    }

    public void OnTileHovered(Tile hoveredTile)
    {
        this.hoveredTile = hoveredTile;

        foreach (var state in globalGameStates)
        {
            state.OnTileHovered(hoveredTile);
        }

        if (state != null) state.OnTileHovered(hoveredTile);
    }

    public void OnTileSelected(Tile selectedTile)
    {
        this.selectedTile = selectedTile;

        foreach (var state in globalGameStates)
        {
            state.OnTileSelected(selectedTile);
        }

        if (state != null) state.OnTileSelected(selectedTile);
    }

    public void OnDeselect()
    {
        selectedTile = null;
        
        foreach (var state in globalGameStates)
        {
            state.OnDeselect();
        }

        if (state != null) state.OnDeselect();
    }
}
