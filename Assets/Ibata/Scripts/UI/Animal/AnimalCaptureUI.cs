using UnityEngine;
using UnityEngine.UI;

public class AnimalCapturedUI : MonoBehaviour
{
    [SerializeField] private AnimalStatusManager manager;
    [SerializeField] private AnimalData targetAnimal;

    // 数字スプライトセット（0~9）
    [SerializeField] private NumberSpriteSet numberSprites;

    [SerializeField] private Image image;   // Text の代わりに Image

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

        // 範囲外対策
        if (count < 0) count = 0;
        if (count >= numberSprites.digits.Length)
            count = numberSprites.digits.Length - 1;

        image.sprite = numberSprites.digits[count];
    }
}