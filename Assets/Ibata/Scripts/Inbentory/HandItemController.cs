using UnityEngine;

public class HandItemController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private Transform holdPoint;

    private ThrowableObject currentInstance;

    void OnEnable()
    {
        inventory.OnCurrentItemChanged += UpdateHandItem;
    }

    void OnDisable()
    {
        inventory.OnCurrentItemChanged -= UpdateHandItem;
    }

    void UpdateHandItem(FruitsData data)
    {
        SetHandItem(data?.handPrefab);
    }

    public void SetHandItem(GameObject prefab)
    {
        // 既存の手持ちを削除
        if (currentInstance != null)
        {
            Destroy(currentInstance.gameObject);
            currentInstance = null;
        }

        if (prefab == null)
            return;

        // 新しい手持ちを生成
        GameObject obj = Instantiate(prefab);
        currentInstance = obj.GetComponent<ThrowableObject>();

        //  元のスケールを取得
        Vector3 originalScale = obj.transform.localScale;

        //  手に持つときは 1/2 に縮小
        obj.transform.localScale = originalScale * 0.5f;

        // 手に持つ処理
        currentInstance.Hold(holdPoint);
    }

    public ThrowableObject CurrentInstance => currentInstance;
}