using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 9f;

    public float OverrideSpeed { get; set; } = -1f;
    public Vector3 HorizontalVelocity { get; private set; }

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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

        // --- ダッシュ ---
        bool dashKey = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        bool dashStick = Gamepad.current != null && Gamepad.current.leftStickButton.isPressed;

        float speed = (dashKey || dashStick) ? dashSpeed : walkSpeed;

        // --- しゃがみ速度が優先 ---
        if (OverrideSpeed > 0)
            speed = OverrideSpeed;

        // --- 移動方向（速度ベクトル） ---
        HorizontalVelocity =
            (transform.forward * input.y + transform.right * input.x) * speed;
    }
}