using System;
using UnityEngine;
using GravityGoat.ScriptableObject.GameEvents;


[CreateAssetMenu(fileName = "New TileObject Game Event", menuName = "Scriptable Object/Event/TileObject Game Event", order = Int32.MaxValue)]
public class TileObjectGameEvent : GameEvent<TileObject>
{
}