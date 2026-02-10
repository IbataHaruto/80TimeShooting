using UnityEngine;
using UnityEngine.UI;

public class CaptureItemUI : MonoBehaviour
{
    [SerializeField] private ThrowItemInventory inventory;
    [SerializeField] private Text countText;

    private void Start()
    {
        countText.text = $"Å~{inventory.CaptureItemCount}";
        inventory.OnCountChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnCountChanged -= UpdateUI;
        }
    }

    private void UpdateUI(int count)
    {
        countText.text = $"Å~{count}";
    }
}
