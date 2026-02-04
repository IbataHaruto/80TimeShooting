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

    [Header("Ceiling Check")]
    [SerializeField] private float ceilingCheckDistance = 0.1f;
    [SerializeField] private LayerMask ceilingMask;

    private CharacterController controller;
    private PlayerMovement movement;

    private Vector3 verticalVelocity;

    // しゃがみ解除直後の「疑似落下補正」を無効化するフラグ
    private bool suppressGroundSnap = false;

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
        //  ポーズ中はジャンプ・重力・移動を完全停止
        if (GameStateManager.IsPaused)
        {
            verticalVelocity = Vector3.zero;
            return;
        }

        HandleJump();
        ApplyGravity();
        MoveCharacter();
    }

    public void SuppressGroundSnap()
    {
        suppressGroundSnap = true;
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

    //  天井チェック
    bool IsCeilingHit()
    {
        Vector3 origin = transform.position + Vector3.up * (controller.height / 2f);
        return Physics.Raycast(origin, Vector3.up, ceilingCheckDistance, ceilingMask);
    }

    void HandleJump()
    {
        bool grounded = IsGrounded();

        if (grounded && verticalVelocity.y < 0)
        {
            if (!suppressGroundSnap)
                verticalVelocity.y = -2f;

            suppressGroundSnap = false;
        }

        bool jumpKey = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool jumpButton = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        if ((jumpKey || jumpButton) && grounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        //  天井に当たったら上昇力を即停止（埋まり防止の核心）
        if (verticalVelocity.y > 0 && IsCeilingHit())
        {
            verticalVelocity.y = 0f;
        }

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