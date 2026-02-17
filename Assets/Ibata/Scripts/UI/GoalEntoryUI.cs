using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalEntryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;

    private AnimalData data;
    private int required;
    private AnimalStatusManager manager;

    public void Initialize(AnimalStatusManager manager, AnimalData data, int required)
    {
        this.manager = manager;
        this.data = data;
        this.required = required;

        iconImage.sprite = data.icon;

        UpdateCount();
    }

    public void UpdateCount()
    {
        int current = manager.GetCapturedCount(data);
        countText.text = $"{current} / {required}";
    }
}