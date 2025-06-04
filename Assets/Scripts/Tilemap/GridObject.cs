using UnityEngine;

public class GridObject : MonoBehaviour
{
    private GridObjectMap gridObjectMap;

    void Awake()
    {
        gridObjectMap = FindAnyObjectByType<GridObjectMap>();
        if (gridObjectMap == null)
        {
            Debug.LogError("GridObjectMap not found in the scene. Please ensure a GridObjectMap component is present.");
        }
    }

    void Start()
    {
        gridObjectMap.Add(this);
    }
}