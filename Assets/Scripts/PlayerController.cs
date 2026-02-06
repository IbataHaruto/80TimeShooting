using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IVelocityAffectable
{
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] InputActionReference moveAction;

    Rigidbody rb;

    Vector3 targetVelocity;
    bool isJumped;

    public Vector2 MoveInput { get; private set; }
    public float CurrentSpeed { get; private set; }
    public IVelocityAffecter Affecter { get; set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Start()
    {
        CurrentSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        MoveInput = moveAction.action.ReadValue<Vector2>();

        targetVelocity = new Vector3(
            MoveInput.x,
            0.0f,
            MoveInput.y
        ) * CurrentSpeed;

        // 外部から速度影響を受ける場合
        if (Affecter != null)
        {
            Vector3 affected = Affecter.AffectedVelocity;
            affected.y = 0.0f;
            targetVelocity += affected;
        }

        rb.linearVelocity = targetVelocity;

    }

    void Rotate()
    {
        if (targetVelocity.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(targetVelocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isJumped)
        {
            isJumped = false;
            CurrentSpeed = moveSpeed;
            StartCoroutine(DisableMoveInputCoroutine());
        }
    }

    IEnumerator DisableMoveInputCoroutine()
    {
        moveAction.action.Disable();
        yield return new WaitForSeconds(0.5f);
        moveAction.action.Enable();
    }
}


