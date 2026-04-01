using UnityEngine;
using UnityEngine.EventSystems;

public class SliderSelectScale : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] Vector3 selectScale = Vector3.one * 5f;

    Vector3 defaultScale;

    void Awake()
    {
        // 元のサイズを保存
        defaultScale = transform.localScale;
      
    }

    public void OnSelect(BaseEventData eventData)
    {
        transform.localScale = selectScale;
        
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale;
    }

    void OnDisable()
    {
        transform.localScale = defaultScale;
    }
}
