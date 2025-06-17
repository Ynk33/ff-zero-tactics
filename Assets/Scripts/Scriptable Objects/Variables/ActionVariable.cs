using GravityGoat.ScriptableObject.Variables;
using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Action Variable", menuName = "Scriptable Object/Variable/Action", order = Int32.MaxValue)]
public class ActionVariable : GenericVariable<Action>
{
    public static implicit operator Action(ActionVariable variable) => variable.Value;
}