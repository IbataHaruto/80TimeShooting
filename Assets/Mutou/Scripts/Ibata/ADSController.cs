using UnityEngine;
using UnityEngine.InputSystem;

public class ADSController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float adsFov = 40f;
    [SerializeField] private float normalFov = 60f;

    public static bool IsADS = false;

    void Update()
    {
        //  ポーズ中は ADS を完全無効化
        if (GameStateManager.IsPaused)
        {
            IsADS = false; // ポーズに入った瞬間 ADS を解除
            cam.fieldOfView = normalFov;
            return;
        }

        // --- マウス ---
        if (Mouse.current != null)
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
                IsADS = true;

            if (Mouse.current.rightButton.wasReleasedThisFrame)
                IsADS = false;
        }

        // --- ゲームパッド ---
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftTrigger.wasPressedThisFrame)
                IsADS = true;

            if (Gamepad.current.leftTrigger.wasReleasedThisFrame)
                IsADS = false;
        }

        cam.fieldOfView = IsADS ? adsFov : normalFov;
    }
}