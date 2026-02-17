using UnityEngine;
using TMPro;

public class AnimalCapturedUI : MonoBehaviour
{
    [SerializeField] private AnimalStatusManager manager;
    [SerializeField] private AnimalData targetAnimal;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        UpdateUI();
        manager.OnCaptured += OnCaptured;
    }

    private void OnCaptured(AnimalData data)
    {
        if (data == targetAnimal)
            UpdateUI();
    }

    private void UpdateUI()
    {
        int count = manager.GetCapturedCount(targetAnimal);
        text.text = $"{targetAnimal.animalName}: {count}";
    }
}