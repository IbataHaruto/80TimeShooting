using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private InventoryUI slotUIPrefab;
    [SerializeField] private Transform slotRoot;

    private InventoryUI[] uiSlots;

    void Start()
    {
        uiSlots = new InventoryUI[inventory.SlotCount];

        for (int i = 0; i < inventory.SlotCount; i++)
        {
            var ui = Instantiate(slotUIPrefab, slotRoot);

            //  生成位置をずらす（横に150px間隔）
            RectTransform rt = ui.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * 150f, 0f);

            //  最前列に配置（他のUIに隠れないように）
            ui.transform.SetAsLastSibling();

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
    }
}