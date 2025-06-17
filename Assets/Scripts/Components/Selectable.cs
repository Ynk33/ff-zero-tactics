using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    bool isSelectable = true;

    [SerializeField]
    MMF_Player onHoverFeedback;

    [SerializeField, ShowIf("isSelectable")]
    MMF_Player onSelectFeedback;

    bool hovered = false;
    bool selected = false;

    public void Hover(bool isHovered)
    {
        hovered = isHovered;
        
        if (isHovered)
        {
            if (!selected) onHoverFeedback.PlayFeedbacks(); // Don't play feedbacks if selected!
        }
        else
        {
            if (!selected) onHoverFeedback.RestoreInitialValues(); // Don't play feedbacks if selected!
        }
    }

    public void Select(bool isSelected)
    {
        if (!isSelectable) return;

        selected = isSelected;

        if (isSelected)
        {
            onSelectFeedback.PlayFeedbacks();
        }
        else if (hovered)
        {
            // Keep the hovered feedbacks playing if hovered.
            onHoverFeedback.PlayFeedbacks();
        }
        else
        {
            // If not selected nor hovered, restore initial values.
            onHoverFeedback.RestoreInitialValues();
        }
    }
}
