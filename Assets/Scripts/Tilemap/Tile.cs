using MoreMountains.Feedbacks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    MMF_Player overlayFeedback;

    [SerializeField]
    MMF_Player arrowFeedback;

    void Update()
    {
        HideArrow();
    }

    public void ShowOverlay()
    {
        if (overlayFeedback != null)
        {
            overlayFeedback.PlayFeedbacks();
        }
    }

    public void HideOverlay()
    {
        if (overlayFeedback != null)
        {
            overlayFeedback.StopFeedbacks();
        }
    }

    public void ShowArrow()
    {
        if (arrowFeedback != null)
        {
            arrowFeedback.PlayFeedbacks();
        }
    }

    public void HideArrow()
    {
        if (arrowFeedback != null)
        {
            arrowFeedback.StopFeedbacks();
        }
    }
}
