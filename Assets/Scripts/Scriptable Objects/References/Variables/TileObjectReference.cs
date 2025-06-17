using System;
using GravityGoat.ScriptableObject.References;
using UnityEngine;


[Serializable]
public class TileObjectReference : ScriptableReference
{
    [SerializeField]
    bool useConstant = true;

    public TileObject Constant = default;
    public TileObjectVariable Reference = default;

    public TileObject Value
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

    public static implicit operator TileObject(TileObjectReference reference) => reference.Value;
}