using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCursors : MonoBehaviour, IPointerEnterHandler,
IPointerExitHandler
{
    [SerializeField] private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f); // マウスオーバー時のサイズ
    [SerializeField] private float scaleSpeed = 10f; // 補間速度

    private Vector3 originalScale; // 元のサイズ
    private Vector3 targetScale;   // 現在の目標サイズ

    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        // スムーズにサイズを補間
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);
    }

    // マウスが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = hoverScale;
    }

    // マウスが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

}
