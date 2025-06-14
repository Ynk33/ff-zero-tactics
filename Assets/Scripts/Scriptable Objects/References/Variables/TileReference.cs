using System;
using GravityGoat.ScriptableObject.References;
using UnityEngine;


[Serializable]
public class TileReference : ScriptableReference
{
    [SerializeField]
    bool useConstant = true;

    public Tile Constant = default;
    public TileVariable Reference = default;

    public Tile Value
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

    public static implicit operator Tile(TileReference reference) => reference.Value;
}