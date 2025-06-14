using GravityGoat.ScriptableObject.Variables;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tile Variable", menuName = "Scriptable Object/Variable/Tile", order = Int32.MaxValue)]
public class TileVariable : GenericVariable<Tile>
{
    public static implicit operator Tile(TileVariable variable) => variable.Value;
}