using GravityGoat.ScriptableObject.Variables;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New GridManager Variable", menuName = "Scriptable Object/Variable/GridManager", order = Int32.MaxValue)]
public class GridManagerVariable : GenericVariable<GridManager>
{
    public static implicit operator GridManager(GridManagerVariable variable) => variable.Value;
}