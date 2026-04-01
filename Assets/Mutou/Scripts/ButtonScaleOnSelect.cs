using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Button))]

public class ButtonScaleOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("拡大倍率")]
    public float selectedScale = 1.2f; // 選択時の倍率
    public float normalScale = 1.0f;   // 通常時の倍率
    public float animationSpeed = 10f; // 補間速度

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = Vector3.one * normalScale;
        transform.localScale = targetScale;
    }

    private void Update()
    {
        // スムーズに拡大縮小
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    // 選択されたとき（コントローラーやキーボードで移動したときも呼ばれる）
    public void OnSelect(BaseEventData eventData)
    {
        targetScale = Vector3.one * selectedScale;
    }

    // 選択解除されたとき
    public void OnDeselect(BaseEventData eventData)
    {
        targetScale = Vector3.one * normalScale;
    }
}
