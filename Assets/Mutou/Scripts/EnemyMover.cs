using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class EnemyMover : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float walkSpeed = 12f;
    [SerializeField] float runSpeed = 20f;

    [SerializeField] Transform target;

    [Header("Detection Ranges")]
    [SerializeField] float normalDetectRange = 10f;
    [SerializeField] float stealthIgnoreRange = 4f;

    [Header("State Cooldown")]
    [SerializeField] float stateChangeCooldown = 3.0f; // 状態切り替えの最小間隔
    private float stateTimer = 0f;

    [Header("RunAway Direction Change")]
    [SerializeField] float changeDirectionMin = 1.5f;
    [SerializeField] float changeDirectionMax = 3f;

    [Header("Idle Direction Change")]
    [SerializeField] float idleChangeMin = 2f;
    [SerializeField] float idleChangeMax = 4f;

    private float changeTimer = 0f;
    private float changeInterval = 0f;

    private bool playerCrouch = false;
    private bool isRunningAway = false;

    private Vector3 runDirection;
    private Vector3 wanderDirection;

    //追加
    [SerializeField] Rigidbody rb;
    public float maxSlopeAngle = 45f;
    private bool onSlope;
    private Vector3 slopeMoveDir;


    private void Start()
    {
        rb.freezeRotation = true; // 回転防止
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        float dis = Vector3.Distance(transform.position, target.position);

        Vector3 moveDir = new Vector3(0, 0,0 ).normalized;

        // 地面チェック
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.2f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            onSlope = slopeAngle > 0 && slopeAngle <= maxSlopeAngle;

            if (onSlope)
            {
                slopeMoveDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
            }
        }

        // 坂道補正
        Vector3 finalMove = onSlope ? slopeMoveDir : moveDir;
        rb.MovePosition(rb.position + finalMove * runSpeed * Time.deltaTime);

        // --- 逃走継続中 ---
        if (isRunningAway)
        {
                // クールダウンが終わっていて、距離が離れたら Idle に戻る
                if (stateTimer >= stateChangeCooldown &&
                    dis > normalDetectRange)
                {
                    isRunningAway = false;
                    stateTimer = 0f;
                    return;
                }

                RunAway();
                return;
        }

        // --- Idle → RunAway の切り替え（クールダウン付き）---
        if (stateTimer >= stateChangeCooldown)
        {
                if (dis <= stealthIgnoreRange)
                {
                    StartRunAway();
                    return;
                }

                if (dis <= normalDetectRange && !playerCrouch)
                {
                    StartRunAway();
                    return;
                }
        }

        // --- Idle ---
        IdleWalk();
    }
    private void StartRunAway()
    {
        if (!isRunningAway)
        {
            isRunningAway = true;
            stateTimer = 0f;

            DecideRunDirection();
            changeInterval = Random.Range(changeDirectionMin, changeDirectionMax);
            changeTimer = 0f;
        }

        RunAway();
    }

    private void DecideRunDirection()
    {
        Vector3 toPlayer = (target.position - transform.position);
        toPlayer.y = 0;

        Vector3 baseDir = -toPlayer.normalized;

        float angle = Random.Range(-80f, 80f);
        runDirection = Quaternion.Euler(0, angle, 0) * baseDir;

        if (runDirection.sqrMagnitude < 0.001f)
            runDirection = transform.right;

        runDirection.Normalize();
    }

    private void DecideWanderDirection()
    {
        float angle = Random.Range(0f, 360f);
        wanderDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        wanderDirection.Normalize();
    }

    public void RunAway()
    {
        changeTimer += Time.deltaTime;

        if (changeTimer >= changeInterval)
        {
            DecideRunDirection();
            changeInterval = Random.Range(changeDirectionMin, changeDirectionMax);
            changeTimer = 0f;
        }

        transform.rotation = Quaternion.LookRotation(runDirection);
        transform.position += runDirection * runSpeed * Time.deltaTime;
    }
    public void PlayerShift()
    {
        playerCrouch = true;
    }
    public void DontPlayerShift()
    {
        playerCrouch = false;
    }

    private void IdleWalk()
    {
        if (wanderDirection == Vector3.zero)
        {
            DecideWanderDirection();
            changeInterval = Random.Range(idleChangeMin, idleChangeMax);
            changeTimer = 0f;
        }

        changeTimer += Time.deltaTime;

        if (changeTimer >= changeInterval)
        {
            DecideWanderDirection();
            changeInterval = Random.Range(idleChangeMin, idleChangeMax);
            changeTimer = 0f;
        }

        transform.rotation = Quaternion.LookRotation(wanderDirection);
        transform.position += wanderDirection * walkSpeed * Time.deltaTime;
    }

    public void PlayerCrouch() => playerCrouch = true;
    public void DontPlayerCrouch() => playerCrouch = false;
}