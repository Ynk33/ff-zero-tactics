using UnityEngine;
using UnityEngine.Events;

public class Highlightable : MonoBehaviour
{
    [SerializeField]
    UnityEvent onHighlight;

    [SerializeField]
    UnityEvent onResetColor;

    void Update()
    {
        ResetColor();
    }

    public void Highlight()
    {
        onHighlight?.Invoke();
    }

    public void ResetColor()
    {
        onResetColor?.Invoke();
    }
}
