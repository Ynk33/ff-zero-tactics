using System;
using GravityGoat.ScriptableObject.References;
using UnityEngine;


[Serializable]
public class UnitReference : ScriptableReference
{
    [SerializeField]
    bool useConstant = true;

    public Unit Constant = default;
    public UnitVariable Reference = default;

    public Unit Value
    {
      get => useConstant ? Constant : Reference != null ? Reference.Value : default;
      set
      {
          if (useConstant)
          {
              Constant = value;
          }
          else if (Reference != null)
          {
              Reference.Value = value;
          }
      }
    }

    public static implicit operator Unit(UnitReference reference) => reference.Value;
}