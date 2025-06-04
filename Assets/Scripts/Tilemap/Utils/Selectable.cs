using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    UnityEvent onSelect;

    [SerializeField]
    UnityEvent onDeselect;

    public void Select()
    {
        // Implement selection logic here
        Debug.Log($"{gameObject.name} selected.");
        onSelect?.Invoke();
    }

    public void Deselect()
    {
        // Implement deselection logic here
        Debug.Log($"{gameObject.name} deselected.");
        onDeselect?.Invoke();
    }
}
