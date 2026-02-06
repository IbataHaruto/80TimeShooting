using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -30f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private LayerMask groundMask;

    [Header("Ceiling Check")]
    [SerializeField] private float ceilingCheckDistance = 0.1f;
    [SerializeField] private LayerMask ceilingMask;

    private CharacterController controller;
    private PlayerMovement movement;

    private Vector3 verticalVelocity;

    private bool suppressGroundSnap = false;

    private const float maxFallSpeed = -75f;

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
        if (GameStateManager.IsPaused)
        {
            verticalVelocity = Vector3.zero;
            return;
        }

        HandleJump();
        ApplyGravity();
        MoveCharacter();
    }

    //  PlayerCrouch から呼び出す
    public void SetGroundCheckDistance(float value)
    {
        groundCheckDistance = value;
    }

    //  PlayerCrouch から呼び出す（坂吸着抑制）
    public void SuppressGroundSnap()
    {
        suppressGroundSnap = true;
    }

    //  pivot が足元でも height/radius が変化しても破綻しない GroundCheck
    bool IsGrounded()
    {
        // CharacterController の底面ワールド座標
        float bottomWorldY =
            transform.position.y +
            controller.center.y -
            (controller.height * 0.5f) +
            controller.radius;

        // 底面から少し上に origin を置く
        Vector3 origin = new Vector3(
            transform.position.x,
            bottomWorldY + 0.05f,
            transform.position.z
        );

        bool grounded = Physics.SphereCast(
            origin,
            controller.radius * 0.9f,
            Vector3.down,
            out _,
            groundCheckDistance,
            groundMask
        );

       // Debug.Log($"[GroundCheck] grounded={grounded} origin={origin}");
        Debug.DrawRay(origin, Vector3.down * groundCheckDistance, grounded ? Color.green : Color.red);

        return grounded;
    }

    //  pivot 足元でも height 変化に対応した CeilingCheck
    bool IsCeilingHit()
    {
        float topWorldY =
            transform.position.y +
            controller.center.y +
            (controller.height * 0.5f) -
            controller.radius;

        Vector3 origin = new Vector3(
            transform.position.x,
            topWorldY - 0.05f,
            transform.position.z
        );

        bool hit = Physics.Raycast(origin, Vector3.up, ceilingCheckDistance, ceilingMask);

       // Debug.Log($"[CeilingCheck] hit={hit} origin={origin}");
        //Debug.DrawRay(origin, Vector3.up * ceilingCheckDistance, hit ? Color.yellow : Color.blue);

        return hit;
    }

    void HandleJump()
    {
        bool grounded = IsGrounded();

        bool jumpKey = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpButton = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if ((jumpKey || jumpButton) && grounded)
        {
            suppressGroundSnap = true;

            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

           // Debug.Log($"[Jump] Jump triggered! verticalVelocity={verticalVelocity.y}");
        }

        if (grounded && verticalVelocity.y < 0)
        {
           // Debug.Log($"[Landing] grounded={grounded} suppress={suppressGroundSnap}");

            if (!suppressGroundSnap)
                verticalVelocity.y = -3f;
            else
                verticalVelocity.y = 0f;

            suppressGroundSnap = false;
        }
    }

    void ApplyGravity()
    {
        if (verticalVelocity.y > 0 && IsCeilingHit())
        {
           // Debug.Log("[Ceiling] Hit ceiling → verticalVelocity reset");
            verticalVelocity.y = 0f;
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        verticalVelocity.y = Mathf.Clamp(verticalVelocity.y, maxFallSpeed, 50f);

        //Debug.Log($"[Gravity] verticalVelocity={verticalVelocity.y}");
    }

    void MoveCharacter()
    {
        Vector3 move = movement.HorizontalVelocity;

        if (IsGrounded() && verticalVelocity.y <= 0 && !suppressGroundSnap)
        {
            //Debug.Log("[SlopeSnap] Applying slope snap");
            move += Vector3.down * 1.0f;
        }

        move += new Vector3(0, verticalVelocity.y, 0);

        controller.Move(move * Time.deltaTime);
    }
}