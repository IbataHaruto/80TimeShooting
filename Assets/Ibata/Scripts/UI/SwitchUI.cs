using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class UIInputSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject keyboardUI;
    [SerializeField] private GameObject gamepadUI;

    void OnEnable()
    {
        InputSystem.onEvent += OnInputEvent;
    }

    void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }

    void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        if (device == null) return;

        // ゲームパッド操作
        if (device is Gamepad)
        {
            ShowGamepadUI();
        }
        // キーボード or マウス操作
        else if (device is Keyboard || device is Mouse)
        {
            ShowKeyboardUI();
        }
    }

    void ShowKeyboardUI()
    {
        keyboardUI.SetActive(true);
        gamepadUI.SetActive(false);
    }

    void ShowGamepadUI()
    {
        keyboardUI.SetActive(false);
        gamepadUI.SetActive(true);
    }
}