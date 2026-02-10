using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickThrow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float pickDistance = 8f;
    [SerializeField] private float pickRadius = 0.5f;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float throwPos = 1.5f;   // 基本距離
    [SerializeField] private float pitchAdjust = -10f; // 射出角度補正

    [SerializeField] private InventoryModel fruitInventory;
    [SerializeField] private ThrowItemInventory throwInventory;
    [SerializeField] private GameObject captureItemPrefab;

    [SerializeField] private HandItemController hand;
    [SerializeField] private CrosshairController crosshair;

    private PlayerInput input;
    private InputAction pickAction;
    private InputAction throwAction;
    private InputAction switchThrowItemAction;

    private bool isThrowItemMode = false;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        pickAction = input.actions["Pick"];
        throwAction = input.actions["Throw"];
        switchThrowItemAction = input.actions["SwitchThrowItem"];
    }

    public void ExitThrowItemMode()
    {
        isThrowItemMode = false;
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        if (switchThrowItemAction.WasPressedThisFrame())
            isThrowItemMode = true;

        // --- クロスヘアのレイ ---
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        // --- 拾い判定 ---
        PickableFruit target = null;
        if (Physics.SphereCast(ray, pickRadius, out RaycastHit hit, pickDistance))
            target = hit.collider.GetComponent<PickableFruit>();

        crosshair.SetCanPick(target != null);

        // --- 拾う ---
        if (target != null && pickAction.WasPressedThisFrame())
        {
            if (fruitInventory.Add(target.data))
            {
                Destroy(target.gameObject);
                ExitThrowItemMode();
            }
        }

        // --- 投げ方向（クロスヘア方向） ---
        Vector3 dir = ray.direction;

        // --- 射出角度補正（上下方向） ---
        dir = Quaternion.AngleAxis(pitchAdjust, cam.transform.right) * dir;

        // --- pitch に応じて距離補正 ---
        float pitch = cam.GetComponent<CameraLook>().Pitch;
        float pitch01 = Mathf.InverseLerp(-60f, 60f, pitch);
        float distanceMul = Mathf.Lerp(1f, 0.3f, pitch01);
        float adjustedThrowPos = throwPos * distanceMul;

        // --- 生成位置 ---
        Vector3 spawnPos = ray.origin + ray.direction * adjustedThrowPos;

        // --- 投げる（果物） ---
        if (throwAction.WasPressedThisFrame() && !isThrowItemMode)
        {
            var data = fruitInventory.CurrentItemData;
            if (data != null)
            {
                var world = Instantiate(data.pickablePrefab, spawnPos, Quaternion.identity);

                var throwable = world.GetComponent<ThrowableObject>();
                throwable.Throw(dir * throwForce);

                fruitInventory.RemoveOne();
            }
        }

        // --- 投げる（捕獲アイテム） ---
        if (throwAction.WasPressedThisFrame() && isThrowItemMode)
        {
            if (!throwInventory.UseOne())
                return;

            var world = Instantiate(captureItemPrefab, spawnPos, Quaternion.identity);

            // ★ 緑軸（up）が閉じている側 → プレイヤー方向（-dir）へ向ける
            world.transform.rotation = Quaternion.FromToRotation(world.transform.up, -dir);

            // 投げ方向（角度補正）
            Vector3 netDir = Quaternion.AngleAxis(10f, cam.transform.right) * dir;

            var throwable = world.GetComponent<ThrowableObject>();
            throwable.Throw(netDir * throwForce);
        }
    }
}