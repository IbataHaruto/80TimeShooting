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

    [Header("Custom Gravity (Fallback)")]
    [SerializeField] private float defaultUpGravity = -9.81f;
    [SerializeField] private float defaultDownGravity = -9.81f;

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
    [SerializeField] private float lineFadeSpeed = 8f;

    private float lineAlpha = 0f;

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
                SetLineAlpha(0f);
            return;
        }

        bool isDash = PlayerStateManager.Instance.CurrentState == PlayerState.Dash;

        UpdateTrajectoryLine(isDash);
        UpdateLineFade(isDash);

        if (hand.CurrentInstance != null)
            hand.CurrentInstance.gameObject.SetActive(!isDash);

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        IInteractable target = null;

        if (Physics.SphereCast(ray, pickRadius, out RaycastHit hit, pickDistance))
            target = hit.collider.GetComponent<IInteractable>();

        crosshair.SetCanPick(target != null);

        if (interactAction.WasPressedThisFrame() && target != null)
        {
            target.Interact();
            return;
        }

        if (isDash)
            return;

        Vector3 dir = ray.direction;
        dir = Quaternion.AngleAxis(pitchAdjust, cam.transform.right) * dir;

        float pitch = cam.GetComponent<CameraLook>().Pitch;
        float pitch01 = Mathf.InverseLerp(-60f, 60f, pitch);
        float distanceMul = Mathf.Lerp(1f, 0.3f, pitch01);
        float adjustedThrowPos = throwPos * distanceMul;

        Vector3 spawnPos = ray.origin + ray.direction * adjustedThrowPos;

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

        if (messageTimer > 0f)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
                inventoryMessageText.alpha = 0f;
        }
    }

    // ================================
    // 軌道描画
    // ================================
    private void UpdateTrajectoryLine(bool isDash)
    {
        if (trajectoryLine == null)
            return;

        if (ADSController.IsADS && !isDash)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            Vector3 dir = ray.direction;
            dir = Quaternion.AngleAxis(pitchAdjust, cam.transform.right) * dir;

            float pitch = cam.GetComponent<CameraLook>().Pitch;
            float pitch01 = Mathf.InverseLerp(-60f, 60f, pitch);
            float distanceMul = Mathf.Lerp(1f, 0.3f, pitch01);
            float adjustedThrowPos = throwPos * distanceMul;

            Vector3 spawnPos = ray.origin + ray.direction * adjustedThrowPos;

            float upG = defaultUpGravity;
            float downG = defaultDownGravity;

            if (hand.CurrentInstance != null)
            {
                var to = hand.CurrentInstance.GetComponent<ThrowableObject>();
                if (to != null)
                {
                    upG = to.upwardGravity;
                    downG = to.downwardGravity;
                }
            }

            float force = isCaptureMode ? captureThrowForce : throwForce;

            DrawTrajectory(spawnPos, dir * force, upG, downG);
        }
    }

    private void DrawTrajectory(Vector3 startPos, Vector3 startVelocity, float upG, float downG)
    {
        trajectoryLine.positionCount = trajectoryPoints;

        Vector3 pos = startPos;
        Vector3 vel = startVelocity;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            trajectoryLine.SetPosition(i, pos);

            float g = vel.y > 0 ? upG : downG;

            vel += Vector3.up * g * Time.fixedDeltaTime;
            pos += vel * Time.fixedDeltaTime;
        }
    }

    // ================================
    // LineRenderer の透明度フェード（修正版）
    // ================================
    private void UpdateLineFade(bool isDash)
    {
        float target = (ADSController.IsADS && !isDash) ? 1f : 0f;

        lineAlpha = Mathf.MoveTowards(lineAlpha, target, Time.deltaTime * lineFadeSpeed);
        SetLineAlpha(lineAlpha);
    }

    // LineRenderer の透明度を start/endColor で変更（最重要修正）
    private void SetLineAlpha(float alpha)
    {
        if (trajectoryLine == null) return;

        Color sc = trajectoryLine.startColor;
        Color ec = trajectoryLine.endColor;

        sc.a = alpha;
        ec.a = alpha;

        trajectoryLine.startColor = sc;
        trajectoryLine.endColor = ec;
    }

    public void ShowFullMessage()
    {
        inventoryMessageText.text = "インベントリーがいっぱいです";
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