using System;
using UnityEngine;
using GravityGoat.ScriptableObject.GameEvents;


[CreateAssetMenu(fileName = "New Tile Game Event", menuName = "Scriptable Object/Event/Tile Game Event", order = Int32.MaxValue)]
public class TileGameEvent : GameEvent<Tile>
{
}