using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float walkSpeed = 12f;
    [SerializeField] float runSpeed = 20f;

    [SerializeField] Transform target;

    [Header("Detection Ranges")]
    [SerializeField] float normalDetectRange = 10f;
    [SerializeField] float stealthIgnoreRange = 4f;

    [Header("State Cooldown")]
    [SerializeField] float stateChangeCooldown = 3.0f;
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

    public Animal animal;

    // Meal 中フラグ
    private bool isEating = false;

    // Meal アニメが無い動物用フォールバック
    [SerializeField] float mealMaxTime = 2.0f;
    private float mealTimer = 0f;

    public bool CanMove =>
        !animal.isCaptured &&
        animal.currentFullness < animal.manager.maxFullness &&
        !isEating;

    private Vector3 runDirection;
    private Vector3 wanderDirection;

    private NavMeshAgent agent;
    private Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updateRotation = false;
        agent.updateUpAxis = true;
    }

    void Start()
    {
        animal.OnEat += StartMeal;
    }

    void Update()
    {
        // -------------------------
        // Pause 中は完全停止（アニメも停止）
        // -------------------------
        if (GameStateManager.IsPaused)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;

            if (animator != null)
                animator.speed = 0f;

            return;
        }
        else
        {
            if (animator != null)
                animator.speed = 1f;
        }

        // -------------------------
        // Meal 中は完全停止
        // -------------------------
        if (isEating)
        {
            mealTimer += Time.deltaTime;

            // Meal アニメが無い動物用フォールバック
            if (mealTimer >= mealMaxTime)
            {
                Debug.Log("[Enemy] Meal フォールバック終了 → アニメ無し動物");
                EndMeal();
            }

            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            return;
        }

        // -------------------------
        // 動けない条件
        // -------------------------
        if (!CanMove)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            return;
        }

        stateTimer += Time.deltaTime;

        float dis = Vector3.Distance(transform.position, target.position);

        bool playerInRange =
            dis <= stealthIgnoreRange ||
            (!playerCrouch && dis <= normalDetectRange);

        if (playerInRange)
        {
            StartRunAway();
            return;
        }

        if (isRunningAway)
        {
            if (stateTimer >= stateChangeCooldown)
            {
                isRunningAway = false;
                stateTimer = 0f;
            }
            else
            {
                RunAway();
            }
            return;
        }

        IdleWalk();
    }

    // -------------------------
    // Meal 開始
    // -------------------------
    private void StartMeal()
    {
        Debug.Log("[Enemy] StartMeal() 呼ばれた → 食事開始");

        isEating = true;
        animal.isEating = true;

        mealTimer = 0f;

        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    // -------------------------
    // Meal 終了（Animator から呼ぶ or フォールバック）
    // -------------------------
    public void EndMeal()
    {
        Debug.Log("[Enemy] EndMeal() 呼ばれた → 食事終了、移動再開");

        isEating = false;
        animal.isEating = false;

        agent.isStopped = false;
    }

    // -------------------------
    // RunAway
    // -------------------------
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

    public void RunAway()
    {
        changeTimer += Time.deltaTime;

        if (changeTimer >= changeInterval)
        {
            DecideRunDirection();
            changeInterval = Random.Range(changeDirectionMin, changeDirectionMax);
            changeTimer = 0f;
        }

        agent.speed = runSpeed;
        agent.acceleration = 200f;
        agent.autoBraking = false;

        Vector3 dest = transform.position + runDirection * 15f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 5f, NavMesh.AllAreas))
            dest = hit.position;

        agent.SetDestination(dest);

        if (agent.velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    // -------------------------
    // Idle Walk
    // -------------------------
    private void DecideWanderDirection()
    {
        float angle = Random.Range(0f, 360f);
        wanderDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        wanderDirection.Normalize();
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

        Vector3 toPlayer = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(wanderDirection, toPlayer);

        float speed = (dot > 0.5f) ? walkSpeed * 0.5f : walkSpeed;
        agent.speed = speed;
        agent.acceleration = 100f;

        Vector3 dest = transform.position + wanderDirection * 15f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 10f, NavMesh.AllAreas))
            dest = hit.position;

        agent.SetDestination(dest);

        if (agent.velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    public void PlayerCrouch() => playerCrouch = true;
    public void DontPlayerCrouch() => playerCrouch = false;
}