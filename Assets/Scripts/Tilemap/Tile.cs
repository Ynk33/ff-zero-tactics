using MoreMountains.Feedbacks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    MMF_Player onHighlightFeedback;

    [SerializeField]
    MMF_Player onShowPathFeedback;

    TileObject tileObject = null;
    Vector3 size;

    public Vector3 WorldPosition => transform.position + size * 0.5f;
    public TileObject TileObject
    {
        get => tileObject;
        set => tileObject = value;
    }

    void Start()
    {
        size = new(transform.localScale.x, 0f, transform.localScale.z);
    }

    public void Hover(bool isHovered)
    {
        if (tileObject != null) tileObject.Hover(isHovered);
    }

    public void Select(bool isSelected)
    {
        if (tileObject != null) tileObject.Select(isSelected);
    }

    public void Highlight(bool isHighlighted)
    {
        if (isHighlighted)
        {
            onHighlightFeedback.PlayFeedbacks();
        }
        else
        {
            onHighlightFeedback.StopFeedbacks();
        }
    }

    public void TogglePath(bool isPath)
    {
        if (isPath)
        {
            onShowPathFeedback.PlayFeedbacks();
        }
        else
        {
            onShowPathFeedback.StopFeedbacks();
        }
    }
}
