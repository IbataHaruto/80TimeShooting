using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectScale : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Vector3 selectScale = Vector3.one * 5f;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite selectSprite;

    Image image;
    Vector3 defaultScale;

    void Awake()
    {
        // 元のサイズを保存
        defaultScale = transform.localScale;
        image = GetComponent<Image>();
        image.sprite = normalSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = selectScale;
        image.sprite = selectSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale;
        image.sprite = normalSprite;
    }

    void OnDisable()
    {
        transform.localScale = defaultScale;
        image.sprite = normalSprite;
    }
}
