using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [SerializeField]
    private Renderer render;
    
    [SerializeField]
    private Color highlightColor = Color.white;
    
    private Color initialColor;

    void Start()
    {
        if (render == null)
        {
            // If no Renderer is found, log an error and return
            Debug.LogError("No Renderer component found on the Tile GameObject or its children: " + gameObject.name);
            return;
        }

        initialColor = render.material.GetColor("_BaseColor");
    }

    void Update()
    {
        ResetColor();
    }

    public void Highlight()
    {
        render.material.SetColor("_BaseColor", highlightColor);
    }

    public void ResetColor()
    {
        render.material.SetColor("_BaseColor", initialColor);
    }
}
