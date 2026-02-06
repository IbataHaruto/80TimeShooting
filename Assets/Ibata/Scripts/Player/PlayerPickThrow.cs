using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickThrow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float pickDistance = 8f;
    [SerializeField] private float pickRadius = 0.5f;
    [SerializeField] private float throwForce = 15f;

    [SerializeField] private InventoryModel fruitInventory;      // 果物
    [SerializeField] private ThrowItemInventory throwInventory;  // 捕獲アイテム
    [SerializeField] private GameObject captureItemPrefab;       // 捕獲アイテムPrefab

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
        switchThrowItemAction = input.actions["SwitchThrowItem"]; // 十字キー上
    }

    public void ExitThrowItemMode()
    {
        isThrowItemMode = false;
        Debug.Log("果物インベントリ操作 → 捕獲モード終了");
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        // --- 十字キー上で捕獲モード ON ---
        if (switchThrowItemAction.WasPressedThisFrame())
        {
            isThrowItemMode = true;
            Debug.Log("捕獲アイテムモードに切り替え");
        }

        // --- SphereCast（拾い判定） ---
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        PickableFruit target = null;

        if (Physics.SphereCast(ray, pickRadius, out RaycastHit hit, pickDistance))
            target = hit.collider.GetComponent<PickableFruit>();

        crosshair.SetCanPick(target != null);

        // --- 拾う（果物） ※捕獲モード中でも拾える ---
        if (target != null && pickAction.WasPressedThisFrame())
        {
            if (fruitInventory.Add(target.data))
            {
                Destroy(target.gameObject);

                //  捕獲モードを終了する
                ExitThrowItemMode();
            }
        }

        // --- 投げる（捕獲アイテム） ---
        if (throwAction.WasPressedThisFrame() && isThrowItemMode)
        {
            if (!throwInventory.UseOne())
            {
                Debug.Log("捕獲アイテムがありません");
                return;
            }

            Vector3 spawnPos = cam.transform.position + cam.transform.forward * 0.5f;

            var world = Instantiate(captureItemPrefab, spawnPos, Quaternion.identity);
            var rb = world.GetComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
        }

        // --- 投げる（果物） ---
        if (throwAction.WasPressedThisFrame() && !isThrowItemMode)
        {
            var data = fruitInventory.CurrentItemData;
            if (data != null)
            {
                Vector3 spawnPos = cam.transform.position + cam.transform.forward * 0.5f;

                var world = Instantiate(data.pickablePrefab, spawnPos, Quaternion.identity);
                var rb = world.GetComponent<Rigidbody>();

                rb.isKinematic = false;
                rb.useGravity = true;

                rb.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);

                fruitInventory.RemoveOne();
            }
        }
    }
}