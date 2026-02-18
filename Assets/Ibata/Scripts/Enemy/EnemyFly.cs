using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float flySpeed = 12f;
    [SerializeField] float runSpeed = 20f;

    [SerializeField] Transform target;

    [Header("Detection Ranges")]
    [SerializeField] float normalDetectRange = 10f;
    [SerializeField] float stealthIgnoreRange = 4f;

    [Header("State Cooldown")]
    [SerializeField] float stateChangeCooldown = 3f;
    private float stateTimer = 0f;

    [Header("Direction Change")]
    [SerializeField] float changeMin = 1.5f;
    [SerializeField] float changeMax = 3f;

    private float changeTimer = 0f;
    private float changeInterval = 0f;

    private bool playerCrouch = false;
    private bool isRunningAway = false;

    public Animal animal;

    public bool CanMove =>
        !animal.isCaptured &&
        animal.currentFullness < animal.manager.maxFullness;

    private Vector3 flyDirection;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 空中動物は Meal を無視
        animal.OnEat += () =>
        {
            Debug.Log("[EnemyFly] Meal を無視（空中動物）");
        };
    }

    void Update()
    {
        // Pause 中は完全停止（アニメも停止）
        if (GameStateManager.IsPaused)
        {
            if (animator != null)
                animator.speed = 0f;

            return;
        }
        else
        {
            if (animator != null)
                animator.speed = 1f;
        }

        if (!CanMove)
            return;

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

        IdleFly();
    }

    private void StartRunAway()
    {
        if (!isRunningAway)
        {
            isRunningAway = true;
            stateTimer = 0f;

            DecideRunDirection();
            changeInterval = Random.Range(changeMin, changeMax);
            changeTimer = 0f;
        }

        RunAway();
    }

    private void DecideRunDirection()
    {
        Vector3 toPlayer = (target.position - transform.position);
        Vector3 baseDir = -toPlayer.normalized;

        float angleY = Random.Range(-80f, 80f);
        float angleX = 0f;

        flyDirection = Quaternion.Euler(angleX, angleY, 0) * baseDir;

        flyDirection.y = 0f;

        if (flyDirection.sqrMagnitude < 0.001f)
            flyDirection = transform.right;

        flyDirection.Normalize();
    }

    private void IdleFly()
    {
        if (flyDirection == Vector3.zero)
        {
            DecideIdleDirection();
            changeInterval = Random.Range(changeMin, changeMax);
            changeTimer = 0f;
        }

        changeTimer += Time.deltaTime;

        if (changeTimer >= changeInterval)
        {
            DecideIdleDirection();
            changeInterval = Random.Range(changeMin, changeMax);
            changeTimer = 0f;
        }

        Move(flyDirection, flySpeed);
    }

    private void DecideIdleDirection()
    {
        float angleY = Random.Range(0f, 360f);
        float angleX = 0f;

        flyDirection = Quaternion.Euler(angleX, angleY, 0) * Vector3.forward;

        flyDirection.y = 0f;
        flyDirection.Normalize();
    }

    private void RunAway()
    {
        changeTimer += Time.deltaTime;

        if (changeTimer >= changeInterval)
        {
            DecideRunDirection();
            changeInterval = Random.Range(changeMin, changeMax);
            changeTimer = 0f;
        }

        Move(flyDirection, runSpeed);
    }

    private void Move(Vector3 dir, float speed)
    {
        transform.position += dir * speed * Time.deltaTime;

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public void PlayerCrouch() => playerCrouch = true;
    public void DontPlayerCrouch() => playerCrouch = false;
}