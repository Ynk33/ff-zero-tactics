using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using GravityGoat.ScriptableObject.GameEvents;


public class TileGameEventListener : GameEventListener<Tile>
{
    [Serializable]
    public class InternalEvent : UnityEvent<Tile> { }
    
    [SerializeField, AssetsOnly]
    TileGameEvent gameEvent = default;

    public InternalEvent response = default;

    protected override GameEvent<Tile> GameEvent => gameEvent;
    protected override UnityEvent<Tile> Response => response;
}