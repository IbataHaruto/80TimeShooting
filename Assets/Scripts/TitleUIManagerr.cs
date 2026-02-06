using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;
    [SerializeField] GameObject optionPanel;

    [SerializeField] Button optionButton;
    [SerializeField] Slider optionFirstSlider;
    [SerializeField] Button titleFirstButton;
    void Start()
    {
        OpenTitle();
    }

    public void OpenOption()
    {
        EventSystem.current.SetSelectedGameObject(null);

        optionPanel.SetActive(true);
        titlePanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(optionFirstSlider.gameObject);
    }

    public void OpenTitle()
    {
        EventSystem.current.SetSelectedGameObject(null);

        titlePanel.SetActive(true);
        optionPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(titleFirstButton.gameObject);
    }
}
