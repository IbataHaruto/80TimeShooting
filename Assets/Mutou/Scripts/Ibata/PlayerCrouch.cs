using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 0.3f;   // Å© Ç‡Ç¡Ç∆í·Ç≠ÇµÇΩÇ¢Ç»ÇÁÇ±Ç±Çâ∫Ç∞ÇÈ
    [SerializeField] private float standHeight = 1.5f;
    [SerializeField] private float crouchSpeed = 2.5f;

    private CharacterController controller;
    private PlayerMovement movement;

    public bool IsCrouching { get; private set; } = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleCrouchInput();
        ApplyCrouchState();
    }

    void HandleCrouchInput()
    {
        bool crouchKey = Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame;
        bool crouchButton = Gamepad.current != null && Gamepad.current.rightStickButton.wasPressedThisFrame;

        if (crouchKey || crouchButton)
            IsCrouching = !IsCrouching;

        bool jumpKey = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpButton = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if ((jumpKey || jumpButton) && IsCrouching)
            IsCrouching = false;
    }

    void ApplyCrouchState()
    {
        if (IsCrouching)
        {
            controller.height = crouchHeight;   // Å© center ÇÕêGÇÁÇ»Ç¢
            movement.OverrideSpeed = crouchSpeed;
        }
        else
        {
            controller.height = standHeight;
            movement.OverrideSpeed = -1f;
        }
    }
}