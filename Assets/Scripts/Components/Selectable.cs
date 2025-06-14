using MoreMountains.Feedbacks;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    MMF_Player onHoverFeedback;

    [SerializeField]
    MMF_Player onSelectFeedback;

    bool hovered = false;
    bool selected = false;

    public bool IsHovered => hovered;
    public bool IsSelected => selected;

    public void Hover(bool isHovered)
    {
        if (isHovered == hovered) return; // No change in hover state

        if (isHovered)
        {
            onHoverFeedback.PlayFeedbacks();
        }
        else
        {
            onHoverFeedback.StopFeedbacks();
        }

        hovered = isHovered;
    }

    public void Select(bool isSelected)
    {
        if (isSelected == selected) return; // No change in hover state

        if (isSelected)
        {
            onSelectFeedback.PlayFeedbacks();
        }
        else
        {
            onSelectFeedback.StopFeedbacks();
        }

        selected = isSelected;
    }
}
