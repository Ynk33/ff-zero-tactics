using System;
using GravityGoat.ScriptableObject.References;
using UnityEngine;


[Serializable]
public class ActionReference : ScriptableReference
{
    [SerializeField]
    bool useConstant = true;

    public Action Constant = default;
    public ActionVariable Reference = default;

    public Action Value
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

    public static implicit operator Action(ActionReference reference) => reference.Value;
}