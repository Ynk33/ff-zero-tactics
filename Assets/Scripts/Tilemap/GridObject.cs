using UnityEngine;

public class GridObject : MonoBehaviour
{
    GridManager gridManager;

    void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridObjectMap not found in the scene. Please ensure a GridObjectMap component is present.");
        }
    }

    void Start()
    {
        gridManager.AddObject(this);
    }
}