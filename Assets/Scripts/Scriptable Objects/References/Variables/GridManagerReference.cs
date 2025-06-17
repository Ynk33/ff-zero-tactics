using System;
using GravityGoat.ScriptableObject.References;
using UnityEngine;


[Serializable]
public class GridManagerReference : ScriptableReference
{
    [SerializeField]
    bool useConstant = true;

    public GridManager Constant = default;
    public GridManagerVariable Reference = default;

    public GridManager Value
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

    public static implicit operator GridManager(GridManagerReference reference) => reference.Value;
}