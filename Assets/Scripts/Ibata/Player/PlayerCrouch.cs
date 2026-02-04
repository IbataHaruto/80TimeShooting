using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    public enum CrouchMode
    {
        Hold,
        Toggle
    }

    [Header("Crouch Mode")]
    public CrouchMode crouchMode = CrouchMode.Toggle;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.2f;
    [SerializeField] private float standHeight = 1.5f;

    [SerializeField] private float crouchRadius = 0.1f;
    [SerializeField] private float standRadius = 0.3f;

    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float transitionSpeed = 12f;

    [Header("Ceiling Check")]
    [SerializeField] private float ceilingCheckDistance = 0.05f;
    [SerializeField] private LayerMask ceilingMask;

    private CharacterController controller;
    private PlayerMovement movement;
    private PlayerJump jump;

    private float currentHeight;
    private float currentRadius;

    public bool IsCrouching { get; private set; } = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();

        currentHeight = controller.height;
        currentRadius = controller.radius;
    }

    void Update()
    {
        //  ポーズ中はしゃがみ処理を完全停止
        if (GameStateManager.IsPaused)
            return;

        HandleCrouchInput();
        ApplyCrouchState();
    }
    void HandleCrouchInput()
    {
        bool crouchPressed =
            (Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.rightStickButton.wasPressedThisFrame);

        bool crouchHeld =
            (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed) ||
            (Gamepad.current != null && Gamepad.current.rightStickButton.isPressed);

        // --- Hold モード ---
        if (crouchMode == CrouchMode.Hold)
        {
            if (crouchHeld)
            {
                // 押している間はしゃがむ（天井関係なし）
                IsCrouching = true;
            }
            else
            {
                // ボタンを離した → 立ち上がりたい
                // でも天井があるなら強制しゃがみ継続
                if (!IsCeilingBlocked())
                    IsCrouching = false;
                else
                    IsCrouching = true; //  強制しゃがみ
            }

            return;
        }

        // --- Toggle モード ---
        if (crouchPressed)
        {
            if (IsCrouching && IsCeilingBlocked())
                return;

            IsCrouching = !IsCrouching;
        }

        // --- ジャンプでしゃがみ解除 ---
        bool jumpPressed =
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

        if (jumpPressed && IsCrouching)
        {
            if (!IsCeilingBlocked())
                IsCrouching = false;
        }
    }

    bool IsCeilingBlocked()
    {
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2f);
        return Physics.Raycast(origin, Vector3.up, ceilingCheckDistance, ceilingMask);
    }

    void ApplyCrouchState()
    {
        float targetHeight = IsCrouching ? crouchHeight : standHeight;
        float targetRadius = IsCrouching ? crouchRadius : standRadius;

        //  しゃがみ解除中に天井に当たったら即しゃがみに戻す（強制しゃがみ）
        if (!IsCrouching && IsCeilingBlocked())
        {
            IsCrouching = true;

            // 補間を止めて即しゃがみ値に戻す
            currentHeight = crouchHeight;
            currentRadius = crouchRadius;

            controller.height = currentHeight;
            controller.radius = currentRadius;

            movement.OverrideSpeed = crouchSpeed;
            return;
        }

        if (!IsCrouching)
            jump.SuppressGroundSnap();

        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * transitionSpeed);
        currentRadius = Mathf.Lerp(currentRadius, targetRadius, Time.deltaTime * transitionSpeed);

        controller.height = currentHeight;
        controller.radius = currentRadius;

        movement.OverrideSpeed = IsCrouching ? crouchSpeed : -1f;
    }
}