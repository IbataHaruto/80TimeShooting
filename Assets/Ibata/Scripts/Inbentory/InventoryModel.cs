using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    public List<InventorySlot> Slots { get; } = new();
    public int CurrentIndex { get; private set; } = -1;

    public event Action<FruitsData> OnCurrentItemChanged;
    public event Action OnSlotsChanged;

    public InventorySlot CurrentSlot =>
        (CurrentIndex >= 0 && CurrentIndex < Slots.Count)
        ? Slots[CurrentIndex]
        : null;

    public FruitsData CurrentItemData =>
        CurrentSlot?.Data;

    [SerializeField] private int fruitSlotCount = 3; // 果物スロット数
    public int SlotCount => fruitSlotCount + 1;      // +1 が捕獲スロット

    // 捕獲スロット判定
    public bool IsCaptureSlot(int index) => index == fruitSlotCount;

    // -------------------------
    // Add（果物のみ）
    // -------------------------
    public bool Add(FruitsData data)
    {
        // 既存スロット
        for (int i = 0; i < Slots.Count; i++)
        {
            var slot = Slots[i];
            if (slot.Data == data && slot.CanAddOne())
            {
                slot.AddOne();
                SetIndex(i);
                OnSlotsChanged?.Invoke();
                return true;
            }
        }

        // 新規スロット
        if (Slots.Count < fruitSlotCount)
        {
            Slots.Add(new InventorySlot(data));
            SetIndex(Slots.Count - 1);
            OnSlotsChanged?.Invoke();
            return true;
        }

        return false;
    }

    // -------------------------
    // RemoveOne（果物）
    // -------------------------
    public void RemoveOne()
    {
        var slot = CurrentSlot;
        if (slot == null) return;

        if (!slot.RemoveOne()) return;

        if (slot.IsEmpty)
        {
            int removedIndex = CurrentIndex;
            Slots.RemoveAt(removedIndex);

            if (Slots.Count == 0)
                CurrentIndex = -1;
            else if (removedIndex < Slots.Count)
                CurrentIndex = removedIndex;
            else
                CurrentIndex = Slots.Count - 1;

            OnSlotsChanged?.Invoke();
        }

        NotifyChanged();
    }

    // -------------------------
    // Index 操作
    // -------------------------
    public void SetIndex(int index)
    {
        if (index >= 0 && index < SlotCount)
        {
            if (CurrentIndex == index)
                return; // ← これが重要（同じスロットなら何もしない）

            CurrentIndex = index;
            NotifyChanged();
        }
    }

    public void Next()
    {
        if (SlotCount == 0) return;
        CurrentIndex = (CurrentIndex + 1) % SlotCount;
        NotifyChanged();
    }

    public void Prev()
    {
        if (SlotCount == 0) return;
        CurrentIndex = (CurrentIndex - 1 + SlotCount) % SlotCount;
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        OnCurrentItemChanged?.Invoke(CurrentItemData);
    }
}