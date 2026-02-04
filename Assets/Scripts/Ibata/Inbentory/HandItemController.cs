using UnityEngine;

public class HandItemController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private Transform holdPoint;

    private ThrowableFruit currentInstance;

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
        // 既存の手持ちモデルを削除
        if (currentInstance != null)
        {
            Destroy(currentInstance.gameObject);
            currentInstance = null;
        }

        if (data == null)
            return;

        // 手持ち用のモデルを生成（ワールド用とは別物）
        currentInstance = Instantiate(data.handPrefab); // 手持ち専用Prefabにする

        currentInstance.Hold(holdPoint);
    }

    public ThrowableFruit CurrentInstance => currentInstance;
}