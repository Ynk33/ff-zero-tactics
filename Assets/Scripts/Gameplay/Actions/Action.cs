using UnityEngine;

public abstract class Action : ScriptableObject
{
    [SerializeField]
    protected TileObjectVariable selectedTileObject = default;

    public string actionName = "Action";

    protected abstract bool Validate();

    public abstract bool CanActOn(Tile tile);
    
    public virtual void Prepare()
    {
        if (!Validate())
        {
            throw new UnityException("Error while validating the Action. Check the errors in the console log for more informations.");
        }
    }

    public abstract void Stop();
}
