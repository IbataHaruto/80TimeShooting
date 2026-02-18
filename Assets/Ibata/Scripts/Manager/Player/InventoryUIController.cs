using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private InventoryUI slotPrefab;
    [SerializeField] private Transform slotRoot;
    [SerializeField] private RectTransform selectedFrame;

    private InventoryUI[] uiSlots;

    void Start()
    {
        uiSlots = new InventoryUI[inventory.SlotCount];

        for (int i = 0; i < inventory.SlotCount; i++)
        {
            var ui = Instantiate(slotPrefab, slotRoot);

            RectTransform rt = ui.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * 150f, 0f);

            uiSlots[i] = ui;
        }

        inventory.OnSlotsChanged += RefreshSlots;
        inventory.OnCurrentItemChanged += _ => RefreshSlots();

        RefreshSlots();
    }

    void RefreshSlots()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i < inventory.Slots.Count)
            {
                var slot = inventory.Slots[i];
                uiSlots[i].Set(slot.Data, slot.Count);
            }
            else
            {
                uiSlots[i].Set(null, 0);
            }
        }

        if (inventory.CurrentIndex >= 0)
        {
            selectedFrame.gameObject.SetActive(true);

            RectTransform target = uiSlots[inventory.CurrentIndex].GetComponent<RectTransform>();
            selectedFrame.anchoredPosition = target.anchoredPosition;
        }
        else
        {
            selectedFrame.gameObject.SetActive(false);
        }
    }
}