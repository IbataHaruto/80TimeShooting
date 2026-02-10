using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController controller;
    private PlayerMovement movement;

    private Vector3 verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleJump();
        ApplyGravity();
        MoveCharacter();
    }

    bool IsGrounded()
    {
        Vector3 origin =
            transform.position +
            Vector3.down * (controller.height / 2f - 0.05f);

        bool rayHit = Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );

        return controller.isGrounded || rayHit;
    }

    void HandleJump()
    {
        bool grounded = IsGrounded();

        if (grounded && verticalVelocity.y < 0)
            verticalVelocity.y = -2f;

        bool jumpKey = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpButton = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if ((jumpKey || jumpButton) && grounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        verticalVelocity.y += gravity * Time.deltaTime;
    }

    void MoveCharacter()
    {
        Vector3 finalMove =
            movement.HorizontalVelocity +
            new Vector3(0, verticalVelocity.y, 0);

        controller.Move(finalMove * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        if (controller == null) return;

        Gizmos.color = Color.yellow;

        Vector3 origin =
            transform.position +
            Vector3.down * (controller.height / 2f - 0.05f);

        Gizmos.DrawLine(origin, origin + Vector3.down * groundCheckDistance);
        Gizmos.DrawSphere(origin + Vector3.down * groundCheckDistance, 0.03f);
    }
}