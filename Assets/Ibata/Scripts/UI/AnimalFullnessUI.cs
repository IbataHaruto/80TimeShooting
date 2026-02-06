using UnityEngine;
using UnityEngine.UI;

public class AnimalFullnessUI : MonoBehaviour
{
    [SerializeField] private Animal animal;
    [SerializeField] private Slider fullnessSlider;

    private void Start()
    {
        fullnessSlider.minValue = 0;
        fullnessSlider.maxValue = animal.manager.maxFullness;

        animal.manager.OnFullnessChanged += UpdateUI;

        // 初期反映（最大かどうかは不要なので false を渡す）
        UpdateUI(animal, animal.currentFullness >= animal.manager.maxFullness);
    }

    private void OnDestroy()
    {
        if (animal != null)
            animal.manager.OnFullnessChanged -= UpdateUI;
    }

    private void UpdateUI(Animal changedAnimal, bool isMax)
    {
        if (changedAnimal != animal)
            return;

        fullnessSlider.value = animal.currentFullness;

        // isMax を使ってアニメーションや色変更なども可能
        // 例: fullnessSlider.fillRect.color = isMax ? Color.yellow : Color.white;
    }
}