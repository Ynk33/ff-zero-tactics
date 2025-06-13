using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    UnitDescription unitDescription;

    GridManager gridManager;
    Selectable selectable;

    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure a GridManager component is present.");
        }

        selectable = GetComponent<Selectable>();
        if (selectable != null)
        {
            selectable.onSelect.AddListener(ShowMaxDistance);
            selectable.onDeselect.AddListener(HideMaxDistance);
        }
    }

    void ShowMaxDistance()
    {
        gridManager.HighlightCellsWithinRange(transform.position, unitDescription.maxDistance);
    }

    void HideMaxDistance()
    {
        gridManager.HideAllOverlays();
    }
}
