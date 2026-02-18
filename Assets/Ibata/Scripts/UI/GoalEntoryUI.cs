using UnityEngine;
using UnityEngine.UI;

public class GoalEntryUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    [Header("表示先")]
    [SerializeField] private Image currentImage;   // 捕まえた数
    [SerializeField] private Image requiredImage;  // 目標数

    [Header("数字スプライトセット")]
    [SerializeField] private NumberSpriteSet numberSprites;

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

        SetDigitSprite(currentImage, current);
        SetDigitSprite(requiredImage, required);
    }

    private void SetDigitSprite(Image target, int number)
    {
        number = Mathf.Clamp(number, 0, numberSprites.digits.Length - 1);
        target.sprite = numberSprites.digits[number];
        target.enabled = true;
    }
}