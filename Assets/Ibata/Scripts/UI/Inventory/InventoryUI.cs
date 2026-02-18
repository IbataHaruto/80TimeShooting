using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image countImage;

    // 数字スプライトセット（0~9）
    [SerializeField] private NumberSpriteSet numberSprites;

    /// <summary>
    /// showOne = true の場合、1個でも数字を表示する
    /// </summary>
    public void Set(FruitsData data, int count, bool showOne = false)
    {
        if (data == null)
        {
            icon.enabled = false;
            countImage.enabled = false;
            return;
        }

        icon.enabled = true;
        icon.sprite = data.icon;

        // --- 数字表示ルール ---
        if (!showOne && count <= 1)
        {
            countImage.enabled = false;
            return;
        }

        countImage.enabled = true;

        // 1桁のみ対応（0~9）
        int digit = Mathf.Clamp(count, 0, 9);
        countImage.sprite = numberSprites.digits[digit];
    }
}