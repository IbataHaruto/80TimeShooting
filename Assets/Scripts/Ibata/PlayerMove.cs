using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 9f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isDashing = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        // --- キーボード WASD ---
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

        // --- コントローラー左スティック ---
        if (Gamepad.current != null)
        {
            input += Gamepad.current.leftStick.ReadValue();
        }

        input = Vector2.ClampMagnitude(input, 1f);

        // --- ダッシュ ---
        bool dashKey = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        bool dashStick = Gamepad.current != null && Gamepad.current.leftStickButton.isPressed;
        isDashing = dashKey || dashStick;

        float speed = isDashing ? dashSpeed : walkSpeed;

        // --- 移動 ---
        Vector3 move =
            transform.forward * input.y +
            transform.right * input.x;

        controller.Move(move * speed * Time.deltaTime);

        // --- 独自の地面判定（安定版） ---
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // --- ジャンプ ---
        bool jumpKey = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpButton = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if ((jumpKey || jumpButton) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- 重力 ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}