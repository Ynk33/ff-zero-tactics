using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public UnityEvent onSelect;

    public UnityEvent onDeselect;

    public void Select()
    {
        // Implement selection logic here
        onSelect?.Invoke();
    }

    public void Deselect()
    {
        // Implement deselection logic here
        onDeselect?.Invoke();
    }
}
