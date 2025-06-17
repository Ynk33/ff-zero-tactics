using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI label = default;

    Action action;

    void OnDestroy()
    {
        action.Stop();
    }

    public void Setup(Action action)
    {
        label.text = action.actionName;
        GetComponent<Button>().onClick.AddListener(action.Prepare);

        this.action = action;
    }
}