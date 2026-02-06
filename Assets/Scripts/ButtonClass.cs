using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClass : MonoBehaviour
{
    [SerializeField] Button focusButton;

    void Start()
    {
        // ボタンコンポーネントの取得
        focusButton = focusButton.GetComponent<Button>();
        OnClick();
    }

    public void OnClick()
    {
        //全てのフォーカスを解除する
        EventSystem.current.SetSelectedGameObject(null);
        //focusButtonにフォーカスする
        focusButton.Select();
        //Canvasコンポーネントを無効にする。Buttonコンポーネントで設定可
    }
}
