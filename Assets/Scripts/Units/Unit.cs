using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    UnitDescription unitDescription;

    GridManager gridManager;
    
    bool isSelected = false;

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure a GridManager component is present.");
        }

        if (TryGetComponent<Selectable>(out var selectable))
        {
            selectable.onSelect.AddListener(OnSelected);
            selectable.onDeselect.AddListener(OnDeselected);
        }
    }

    void Update()
    {
        if (isSelected)
        {
            ShowPath();
        }
    }

    void ShowPath()
    {
        Vector3Int hoveredCell = gridManager.HoveredCell;
        if (hoveredCell != GridManager.NO_CELL)
        {
            List<Vector3Int> path = gridManager.FindPath(transform.position, hoveredCell, unitDescription.maxDistance);
            foreach (Vector3Int cell in path)
            {
                gridManager.ShowArrowAt(cell);
            }
        }
    }

    void OnSelected()
    {
        isSelected = true;
        gridManager.ShowCellsWithinRange(transform.position, unitDescription.maxDistance);
    }

    void OnDeselected()
    {
        isSelected = false;
        gridManager.HideAllOverlays();
    }
}
