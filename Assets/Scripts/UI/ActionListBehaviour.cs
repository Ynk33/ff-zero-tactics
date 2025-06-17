using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ActionPanelBehaviour : MonoBehaviour
{
    [SerializeField]
    RectTransform actionList = default;

    [SerializeField]
    ActionButton actionButtonPrefab = default;

    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        ToggleCanvas(false);
    }

    public void OnTileSelected(Tile selectedTile)
    {
        ToggleCanvas(false);

        if (selectedTile == null) return;

        // Fetch the unit on the selected tile
        Unit unit = selectedTile.TileObject as Unit;

        if (unit == null || unit.Actions.Count == 0)
        {
            return;
        }

        ToggleCanvas(true);
        ShowActions(unit);
    }

    void ToggleCanvas(bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.interactable = visible;
    }

    void ShowActions(Unit selectedUnit)
    {
        actionList.ClearChildren();
        foreach (Action action in selectedUnit.Actions)
        {
            ActionButton actionButton = Instantiate(actionButtonPrefab, actionList);
            actionButton.Setup(action);
        }
    }
}
