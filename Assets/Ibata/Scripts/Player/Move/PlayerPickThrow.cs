using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickThrow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float pickDistance = 8f;
    [SerializeField] private float pickRadius = 0.5f;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float captureThrowForce = 8f;
    [SerializeField] private float throwPos = 1.5f;
    [SerializeField] private float pitchAdjust = -10f;

    [Header("Custom Gravity")]
    [SerializeField] private float upwardGravity = -9.81f;
    [SerializeField] private float downwardGravity = -30f;

    [SerializeField] private InventoryModel inventory;
    [SerializeField] private ThrowItemInventory throwInventory;

    [SerializeField] private GameObject captureItemPrefab;
    [SerializeField] private GameObject captureItemHandPrefab;

    [SerializeField] private HandItemController hand;
    [SerializeField] private CrosshairController crosshair;

    [Header("UI")]
    [SerializeField] private TMPro.TextMeshProUGUI inventoryMessageText;

    [Header("Trajectory Preview")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 10;
    [SerializeField] private float timeStep = 0.03f;

    private PlayerInput input;
    private InputAction interactAction;
    private InputAction throwAction;

    private bool isCaptureMode = false;
    private float messageTimer = 0f;

    public InventoryModel Inventory => inventory;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        interactAction = input.actions["Interact"];
        throwAction = input.actions["Throw"];
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
        {
            if (trajectoryLine != null)
                trajectoryLine.enabled = false;
            return;
        }

        bool isDash = PlayerStateManager.Instance.CurrentState == PlayerState.Dash;

        // ============================================================
        //  LineRenderer は ダッシュのリターン の前に必ず更新する
        // ============================================================
        UpdateTrajectoryLine(isDash);

        // ============================================================
        // ここから先は return しても OK（Line は既に更新済み）
        // ============================================================

        // ダッシュ中は手持ちアイテム非表示
        if (hand.CurrentInstance != null)
            hand.CurrentInstance.gameObject.SetActive(!isDash);

        // クロスヘア判定
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        IInteractable target = null;

        if (Physics.SphereCast(ray, pickRadius, out RaycastHit hit, pickDistance))
            target = hit.collider.GetComponent<IInteractable>();

        crosshair.SetCanPick(target != null);

        // Interact
        if (interactAction.WasPressedThisFrame() && target != null)
        {
            target.Interact();
            return;
        }
        //ダッシュ中は投げれない
        if (isDash)
            return;

        // ============================================================
        // 投げ方向計算
        // ============================================================
        Vector3 dir = ray.direction;
        dir = Quaternion.AngleAxis(pitchAdjust, cam.transform.right) * dir;

        float pitch = cam.GetComponent<CameraLook>().Pitch;
        float pitch01 = Mathf.InverseLerp(-60f, 60f, pitch);
        float distanceMul = Mathf.Lerp(1f, 0.3f, pitch01);
        float adjustedThrowPos = throwPos * distanceMul;

        Vector3 spawnPos = ray.origin + ray.direction * adjustedThrowPos;

        // ============================================================
        // 投げる処理
        // ============================================================
        if (throwAction.WasPressedThisFrame())
        {
            if (isCaptureMode)
            {
                if (!throwInventory.UseOne())
                {
                    ExitCaptureMode();
                    return;
                }

                Quaternion rot = Quaternion.LookRotation(cam.transform.forward) * Quaternion.Euler(-90, 0, 0);

                var world = Instantiate(captureItemPrefab, spawnPos, rot);
                var throwable = world.GetComponent<ThrowableObject>();
                throwable.Throw(dir * captureThrowForce);

                if (throwInventory.CaptureItemCount == 0)
                    ExitCaptureMode();
            }
            else
            {
                var data = inventory.CurrentItemData;
                if (data != null)
                {
                    var world = Instantiate(data.pickablePrefab, spawnPos, Quaternion.identity);
                    var throwable = world.GetComponent<ThrowableObject>();
                    throwable.Throw(dir * throwForce);
                    inventory.RemoveOne();
                }
            }
        }

        // メッセージフェードアウト
        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
                inventoryMessageText.alpha = 0f;
        }
    }

    // ============================================================
    // LineRenderer 更新（ダッシュ中でも毎フレーム実行）
    // ============================================================
    private void UpdateTrajectoryLine(bool isDash)
    {
        if (trajectoryLine == null)
            return;

        if (ADSController.IsADS && !isDash)
        {
            trajectoryLine.enabled = true;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Vector3 dir = ray.direction;
            dir = Quaternion.AngleAxis(pitchAdjust, cam.transform.right) * dir;

            float pitch = cam.GetComponent<CameraLook>().Pitch;
            float pitch01 = Mathf.InverseLerp(-60f, 60f, pitch);
            float distanceMul = Mathf.Lerp(1f, 0.3f, pitch01);
            float adjustedThrowPos = throwPos * distanceMul;

            Vector3 spawnPos = ray.origin + ray.direction * adjustedThrowPos;

            float force = isCaptureMode ? captureThrowForce : throwForce;
            DrawTrajectory(spawnPos, dir * force);
        }
        else
        {
            trajectoryLine.enabled = false;
        }
    }

    private void DrawTrajectory(Vector3 startPos, Vector3 startVelocity)
    {
        trajectoryLine.positionCount = trajectoryPoints;

        Vector3 pos = startPos;
        Vector3 vel = startVelocity;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, pos);

            float g = vel.y > 0 ? upwardGravity : downwardGravity;

            vel += Vector3.up * g * timeStep;
            pos += vel * timeStep;
        }
    }

    public void ShowFullMessage()
    {
        inventoryMessageText.text = "Inventory is Full";
        inventoryMessageText.alpha = 1f;
        messageTimer = 1.5f;
    }

    public void EnterCaptureMode()
    {
        if (throwInventory.CaptureItemCount > 0)
        {
            isCaptureMode = true;
            hand.SetHandItem(captureItemHandPrefab);
        }
    }

    public void ExitCaptureMode()
    {
        isCaptureMode = false;
        hand.SetHandItem(inventory.CurrentItemData?.handPrefab);
    }
}