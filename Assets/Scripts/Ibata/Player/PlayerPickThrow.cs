using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickThrow : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float pickDistance = 3f;
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private HandItemController hand;

    private PlayerInput input;
    private InputAction pickAction;
    private InputAction throwAction;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        pickAction = input.actions["Pick"];
        throwAction = input.actions["Throw"];
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        // --- Raycast ---
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        PickableFruit target = null;

        if (Physics.Raycast(ray, out RaycastHit hit, pickDistance))
            target = hit.collider.GetComponent<PickableFruit>();

        // --- 拾う ---
        if (target != null && pickAction.WasPressedThisFrame())
        {
            if (inventory.Add(target.data))
                Destroy(target.gameObject); // ワールド上の実体は破棄
        }

        // --- 投げる ---
        if (throwAction.WasPressedThisFrame())
        {
            var data = inventory.CurrentItemData;
            if (data != null)
            {
                // クロスヘア位置から投げる
                Vector3 spawnPos = cam.transform.position + cam.transform.forward * 0.5f;

                var world = Instantiate(data.worldPrefab, spawnPos, Quaternion.identity);
                world.Throw(cam.transform.forward * 10f);

                inventory.RemoveOne();
            }
        }
    }
}