using UnityEngine;
using UnityEngine.UI;

public class CaptureItemUI : MonoBehaviour
{
    [SerializeField] private ThrowItemInventory inventory;
    [SerializeField] private Image countImage;      // ← Text → Image に変更
    [SerializeField] private Sprite[] numberSprites; // 0~9 のスプライト

    private void Start()
    {
        UpdateUI(inventory.CaptureItemCount);
        inventory.OnCountChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnCountChanged -= UpdateUI;
    }

    private void UpdateUI(int count)
    {
        // 範囲外チェック
        if (count < 0 || count > 9)
            return;

        countImage.sprite = numberSprites[count];
    }
}