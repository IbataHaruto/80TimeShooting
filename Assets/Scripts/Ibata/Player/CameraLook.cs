using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float minPitch = -60f;
    public float maxPitch = 60f;
    private float ControllerPlus = 2.0f;
    private float pitch = 0f;

    private enum ActiveDevice { None, Mouse, Gamepad }
    private ActiveDevice activeDevice = ActiveDevice.None;

    private const float gamepadThreshold = 0.1f; // スティックのデッドゾーン

    void Start()
    {
        // --- FPS モード：カーソルを中央固定 ---
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //  ポーズ中はカメラ操作を完全停止
        if (GameStateManager.IsPaused)
            return;

        float adsMul = ADSController.IsADS
            ? SensitivitySettings.AdsSensitivityMultiplier
            : 1f;

        Vector2 delta = Vector2.zero;

        // --- マウス入力 ---
        Vector2 mouseDelta = Vector2.zero;
        if (Mouse.current != null)
            mouseDelta = Mouse.current.delta.ReadValue();

        // --- コントローラー入力 ---
        Vector2 stick = Vector2.zero;
        if (Gamepad.current != null)
            stick = Gamepad.current.rightStick.ReadValue();

        // --- どちらがアクティブか判定 ---
        if (mouseDelta != Vector2.zero)
        {
            activeDevice = ActiveDevice.Mouse;
        }
        else if (stick.magnitude > gamepadThreshold)
        {
            activeDevice = ActiveDevice.Gamepad;
        }

        // --- アクティブなデバイスだけを使う ---
        switch (activeDevice)
        {
            case ActiveDevice.Mouse:
                delta = mouseDelta * SensitivitySettings.MouseSensitivity * adsMul;
                break;

            case ActiveDevice.Gamepad:
                delta = stick * SensitivitySettings.ControllerSensitivity * adsMul * ControllerPlus;
                break;
        }

        // --- 上下（Pitch） ---
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // --- 左右（Yaw） ---
        if (transform.parent != null)
            transform.parent.Rotate(Vector3.up, delta.x);
    }
}