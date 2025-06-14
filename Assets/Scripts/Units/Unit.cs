using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public delegate void MoveCallback();

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

    public void MoveTo(Vector3Int targetCell, MoveCallback callback)
    {
        List<Vector3Int> path = gridManager.FindPath(transform.position, targetCell, unitDescription.maxDistance);
        if (path.Count > 0)
        {
            // Move the unit along the path
            Sequence moveSequence = DOTween.Sequence();
            foreach (Vector3Int cell in path)
            {
                Vector3 worldPosition = gridManager.GetCellCenterWorldPosition(cell);
                moveSequence.Append(transform.DOMove(worldPosition, 0.1f).SetEase(Ease.Linear));
            }
            moveSequence.OnComplete(() =>
            {
                callback.Invoke();
            });
        }
        else
        {
            callback.Invoke(); // Call the callback even if no path is found
        }
    }

    public float GetCost(Vector3Int from, Vector3Int to)
    {
        // Increase cost if move is forward
        float forwardCost = to.x - from.x;
        if (forwardCost > 0) forwardCost *= 2;

        return Mathf.Abs(forwardCost) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z);
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
