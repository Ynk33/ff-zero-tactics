using DG.Tweening;
using UnityEngine;

public class TileSelectorCursor : MonoBehaviour
{
    [SerializeField]
    float moveDuration = 0.1f;

    public void ShowAt(Vector3 position)
    {
        if (position == transform.position)
        {
            return; // No need to update position if it's the same
        }

        if (!gameObject.activeSelf)
        {
            // If the cursor is not active, set its position and activate it
            gameObject.SetActive(true);
            transform.position = position;
        }
        else
        {
            // If the cursor is already active, animate its movement to the new position
            transform.DOMove(position, moveDuration).SetEase(Ease.OutCubic);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
