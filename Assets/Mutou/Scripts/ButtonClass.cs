using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
