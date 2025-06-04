using UnityEngine;

public class Selectable : MonoBehaviour
{
    public void Select()
    {
        // Implement selection logic here
        Debug.Log($"{gameObject.name} selected.");
    }

    public void Deselect()
    {
        // Implement deselection logic here
        Debug.Log($"{gameObject.name} deselected.");
    }
}
