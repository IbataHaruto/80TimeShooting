using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image frame;     // 450x100
    [SerializeField] private Image icon;      // 100x100
    [SerializeField] private TMP_Text countText;

    public void Set(FruitsData data, int count)
    {
        if (data == null)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = data.icon;

        countText.text = count > 1 ? count.ToString() : "";

        //  アイコンの位置を枠の左から150pxに配置
        //RectTransform rt = icon.GetComponent<RectTransform>();
        //rt.anchoredPosition = new Vector2(0f, 0f);

        //  アイコンを枠の上に表示
        icon.transform.SetAsLastSibling();
        countText.transform.SetAsLastSibling();
    }
}