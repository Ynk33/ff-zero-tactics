using GravityGoat.ScriptableObject.Variables;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Unit Variable", menuName = "Scriptable Object/Variable/Unit", order = Int32.MaxValue)]
public class UnitVariable : GenericVariable<Unit>
{
    public static implicit operator Unit(UnitVariable variable) => variable.Value;
}