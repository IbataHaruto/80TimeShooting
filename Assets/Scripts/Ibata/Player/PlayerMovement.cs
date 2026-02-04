using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float dashSpeed = 11f;

    [SerializeField] private float adsSpeedMultiplier = 0.5f; // ← 追加：ADS 速度低下率

    public float OverrideSpeed { get; set; } = -1f;
    public Vector3 HorizontalVelocity { get; private set; }

    private CharacterController controller;

    private bool isDashToggled = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
        {
            HorizontalVelocity = Vector3.zero;
            return;
        }

        Vector2 input = Vector2.zero;

        // --- キーボード ---
        if (Keyboard.current != null)
        {
            float x = 0f;
            float y = 0f;

            if (Keyboard.current.wKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;

            input += new Vector2(x, y);
        }

        // --- コントローラー ---
        if (Gamepad.current != null)
        {
            input += Gamepad.current.leftStick.ReadValue();
        }

        input = Vector2.ClampMagnitude(input, 1f);

        // --- ダッシュ（トグル） ---
        bool dashKeyPressed =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.wasPressedThisFrame;

        bool dashStickPressed =
            Gamepad.current != null &&
            Gamepad.current.leftShoulder.wasPressedThisFrame;

        if (dashKeyPressed || dashStickPressed)
        {
            isDashToggled = !isDashToggled;
        }

        float speed = isDashToggled ? dashSpeed : walkSpeed;

        // --- ADS 時は移動速度低下 ---
        if (ADSController.IsADS)
            speed *= adsSpeedMultiplier;

        // --- 外部からの速度上書き ---
        if (OverrideSpeed > 0)
            speed = OverrideSpeed;

        HorizontalVelocity =
            (transform.forward * input.y + transform.right * input.x) * speed;
    }
}