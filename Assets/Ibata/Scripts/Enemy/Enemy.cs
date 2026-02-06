using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float walkSpeed = 12f;   // 通常の徘徊速度
    [SerializeField] float runSpeed = 20f;    // 逃走時の速度

    [SerializeField] Transform target;        // プレイヤー参照

    [Header("Detection Ranges")]
    [SerializeField] float normalDetectRange = 10f;   // 通常時の感知距離
    [SerializeField] float stealthIgnoreRange = 4f;   // プレイヤーがしゃがみ中でも無視できない距離

    [Header("State Cooldown")]
    [SerializeField] float stateChangeCooldown = 3.0f; // 逃走 → Idle に戻るまでのクールダウン
    private float stateTimer = 0f;

    [Header("RunAway Direction Change")]
    [SerializeField] float changeDirectionMin = 1.5f;  // 逃走方向の最短変更間隔
    [SerializeField] float changeDirectionMax = 3f;    // 逃走方向の最長変更間隔

    [Header("Idle Direction Change")]
    [SerializeField] float idleChangeMin = 2f;          // Idle の方向変更最短
    [SerializeField] float idleChangeMax = 4f;          // Idle の方向変更最長

    private float changeTimer = 0f;     // 現在の方向維持時間
    private float changeInterval = 0f;  // 次に方向を変えるまでの時間

    private bool playerCrouch = false;  // プレイヤーがしゃがんでいるか
    private bool isRunningAway = false; // 現在逃走状態か

    public Animal animal;

    // 動ける条件：捕獲されていない & 満腹ではない
    public bool CanMove =>
        !animal.isCaptured &&
        animal.currentFullness < animal.manager.maxFullness;

    private Vector3 runDirection;     // 逃走方向
    private Vector3 wanderDirection;  // Idle の徘徊方向

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // 回転はコード側で制御するため NavMeshAgent の回転を無効化
        agent.updateRotation = false;
        agent.updateUpAxis = true;
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        if (!CanMove)
        {
            agent.ResetPath(); // 動けないときは停止
            return;
        }

        stateTimer += Time.deltaTime;

        float dis = Vector3.Distance(transform.position, target.position);

        // プレイヤーが範囲内かどうか（しゃがみ時は遠距離で気づかれにくい）
        bool playerInRange =
            dis <= stealthIgnoreRange ||                      // 10m → 絶対気づく
            (!playerCrouch && dis <= normalDetectRange);      // しゃがんでなければ30mで気づく

        // --- プレイヤーが範囲内なら即逃走 ---
        if (playerInRange)
        {
            StartRunAway();
            return;
        }

        // --- プレイヤーが範囲外 ---
        if (isRunningAway)
        {
            // クールダウン経過で Idle に戻る
            if (stateTimer >= stateChangeCooldown)
            {
                isRunningAway = false;
                stateTimer = 0f;
            }
            else
            {
                RunAway(); // クールダウン中は逃走継続
            }
            return;
        }

        // --- Idle（徘徊） ---
        IdleWalk();
    }

    private void StartRunAway()
    {
        // 状態遷移：Idle → RunAway
        if (!isRunningAway)
        {
            isRunningAway = true;
            stateTimer = 0f;

            DecideRunDirection(); // 初回の逃走方向決定
            changeInterval = Random.Range(changeDirectionMin, changeDirectionMax);
            changeTimer = 0f;
        }

        RunAway();
    }

    private void DecideRunDirection()
    {
        // プレイヤーの反対方向を基準に逃走方向を決定
        Vector3 toPlayer = (target.position - transform.position);
        toPlayer.y = 0;

        Vector3 baseDir = -toPlayer.normalized;

        // ±80度のランダム角度で逃走方向にバリエーションを持たせる
        float angle = Random.Range(-80f, 80f);
        runDirection = Quaternion.Euler(0, angle, 0) * baseDir;

        // 万が一ゼロベクトルなら右方向に逃げる
        if (runDirection.sqrMagnitude < 0.001f)
            runDirection = transform.right;

        runDirection.Normalize();
    }

    private void DecideWanderDirection()
    {
        // Idle の徘徊方向をランダムに決定
        float angle = Random.Range(0f, 360f);
        wanderDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        wanderDirection.Normalize();
    }

    public void RunAway()
    {
        changeTimer += Time.deltaTime;

        // 一定時間ごとに逃走方向を変える
        if (changeTimer >= changeInterval)
        {
            DecideRunDirection();
            changeInterval = Random.Range(changeDirectionMin, changeDirectionMax);
            changeTimer = 0f;
        }

        agent.speed = runSpeed;
        agent.acceleration = 200f;
        agent.autoBraking = false;

        // 逃走方向に25m先を目的地に設定
        Vector3 dest = transform.position + runDirection * 25f;

        // NavMesh 上に補正
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 5f, NavMesh.AllAreas))
            dest = hit.position;

        agent.SetDestination(dest);

        // 進行方向に向けて回転
        if (agent.velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    private void IdleWalk()
    {
        // 初回方向決定
        if (wanderDirection == Vector3.zero)
        {
            DecideWanderDirection();
            changeInterval = Random.Range(idleChangeMin, idleChangeMax);
            changeTimer = 0f;
        }

        changeTimer += Time.deltaTime;

        // Idle の方向変更
        if (changeTimer >= changeInterval)
        {
            DecideWanderDirection();
            changeInterval = Random.Range(idleChangeMin, idleChangeMax);
            changeTimer = 0f;
        }

        // プレイヤー方向に向かって歩くときは速度を落とす（気づかれにくくする演出）
        Vector3 toPlayer = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(wanderDirection, toPlayer);

        float speed = (dot > 0.5f) ? walkSpeed * 0.5f : walkSpeed;
        agent.speed = speed;
        agent.acceleration = 100f;

        // 徘徊先を25m先に設定
        Vector3 dest = transform.position + wanderDirection * 15f;

        // NavMesh 上に補正
        NavMeshHit hit;
        if (NavMesh.SamplePosition(dest, out hit, 10f, NavMesh.AllAreas))
            dest = hit.position;

        agent.SetDestination(dest);

        // 進行方向へ回転
        if (agent.velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
    }

    public void PlayerCrouch() => playerCrouch = true;   // プレイヤーがしゃがんだ
    public void DontPlayerCrouch() => playerCrouch = false; // プレイヤーが立った
}