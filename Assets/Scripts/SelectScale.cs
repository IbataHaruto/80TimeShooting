using UnityEngine;
using UnityEngine.EventSystems;

public class SelectScale : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] float scale = 1.2f;
    Vector3 defaultScale;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    // 選択された瞬間
    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = defaultScale * scale;
    }

    // 選択が外れた瞬間
    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale;
    }
}
