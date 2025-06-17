using GravityGoat.ScriptableObject.Variables;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New TileObject Variable", menuName = "Scriptable Object/Variable/TileObject", order = Int32.MaxValue)]
public class TileObjectVariable : GenericVariable<TileObject>
{
    public static implicit operator TileObject(TileObjectVariable variable) => variable.Value;
}