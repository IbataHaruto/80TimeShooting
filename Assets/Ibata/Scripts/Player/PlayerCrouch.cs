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

    [SerializeField] private float crouchDownSpeed = 35f;
    [SerializeField] private float standUpSpeed = 20f;

    [Header("Ceiling Check")]
    [SerializeField] private float ceilingCheckDistance = 0.05f;
    [SerializeField] private LayerMask ceilingMask;

    private CharacterController controller;
    private PlayerMovement movement;
    private PlayerJump jump;

    private float currentHeight;
    private float currentRadius;

    public event System.Action<bool> OnCrouchStateChanged;

    private bool lastCrouchState = false;

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
        if (GameStateManager.IsPaused)
            return;

        HandleCrouchInput();
        ApplyCrouchState();

        if (IsCrouching != lastCrouchState)
        {
            OnCrouchStateChanged?.Invoke(IsCrouching);
            lastCrouchState = IsCrouching;
        }
    }

    void HandleCrouchInput()
    {
        bool crouchPressed =
            (Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.rightStickButton.wasPressedThisFrame);

        bool crouchHeld =
            (Keyboard.current != null && Keyboard.current.leftCtrlKey.isPressed) ||
            (Gamepad.current != null && Gamepad.current.rightStickButton.isPressed);

        if (crouchMode == CrouchMode.Hold)
        {
            if (crouchHeld)
                IsCrouching = true;
            else
                IsCrouching = !IsCeilingBlocked();

            return;
        }

        if (crouchPressed)
        {
            if (IsCrouching && IsCeilingBlocked())
                return;

            IsCrouching = !IsCrouching;
        }

        bool jumpPressed =
            (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

        if (jumpPressed && IsCrouching && !IsCeilingBlocked())
            IsCrouching = false;
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

        float speed = IsCrouching ? crouchDownSpeed : standUpSpeed;

        //  立ち上がり時に天井がある → 強制しゃがみ
        if (!IsCrouching && IsCeilingBlocked())
        {
            IsCrouching = true;

            currentHeight = crouchHeight;
            currentRadius = crouchRadius;

            controller.height = currentHeight;
            controller.radius = currentRadius;

            movement.OverrideSpeed = crouchSpeed;

            //  しゃがみ時の GroundCheckDistance
            jump.SetGroundCheckDistance(1f);
            return;
        }

        //if (!IsCrouching)
        //    jump.SuppressGroundSnap();

        currentHeight = Mathf.MoveTowards(currentHeight, targetHeight, speed * Time.deltaTime);
        currentRadius = Mathf.MoveTowards(currentRadius, targetRadius, speed * Time.deltaTime);

        controller.height = currentHeight;
        controller.radius = currentRadius;

        movement.OverrideSpeed = IsCrouching ? crouchSpeed : -1f;

        //  GroundCheckDistance の切り替え（ここが今回の追加）
        if (IsCrouching)
            jump.SetGroundCheckDistance(1f);     // しゃがみ時：短い
        else
            jump.SetGroundCheckDistance(2.5f);   // 立ち時：長い
    }
}