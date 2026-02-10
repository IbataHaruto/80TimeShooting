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
        if (currentInstance != null)
        {
            Destroy(currentInstance.gameObject);
            currentInstance = null;
        }

        if (data == null)
            return;

        currentInstance = Instantiate(data.handPrefab);
        currentInstance.Hold(holdPoint);
    }

    public ThrowableFruit CurrentInstance => currentInstance;
}