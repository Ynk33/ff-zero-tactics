using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using GravityGoat.ScriptableObject.GameEvents;


public class TileObjectGameEventListener : GameEventListener<TileObject>
{
    [Serializable]
    public class InternalEvent : UnityEvent<TileObject> { }
    
    [SerializeField, AssetsOnly]
    TileObjectGameEvent gameEvent = default;

    public InternalEvent response = default;

    protected override GameEvent<TileObject> GameEvent => gameEvent;
    protected override UnityEvent<TileObject> Response => response;
}