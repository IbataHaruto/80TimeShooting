using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float minPitch = -60f;
    public float maxPitch = 60f;
    private float ControllerPlus = 2.0f;
    private float pitch = 0f;

    void Update()
    {
        float adsMul = ADSController.IsADS
            ? SensitivitySettings.AdsSensitivityMultiplier
            : 1f;

        Vector2 delta = Vector2.zero;

        // --- マウス ---
        if (Mouse.current != null)
        {
            delta += Mouse.current.delta.ReadValue()
                     * SensitivitySettings.MouseSensitivity
                     * adsMul;
        }

        // --- コントローラー ---
        if (Gamepad.current != null)
        {
            delta += Gamepad.current.rightStick.ReadValue()
                     * SensitivitySettings.ControllerSensitivity
                     * adsMul * ControllerPlus;
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