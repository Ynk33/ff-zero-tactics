using MoreMountains.Feedbacks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    MMF_Player overlayFeedback;

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
}
