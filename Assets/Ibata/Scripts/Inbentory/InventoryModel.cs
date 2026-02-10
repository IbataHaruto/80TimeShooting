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

    [SerializeField] private int slotCount = 3;
    public int SlotCount => slotCount;

    // -------------------------
    // Add
    // -------------------------
    public bool Add(FruitsData data)
    {
        // 1. 既存スロットに追加
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

        // 2. 新規スロット
        if (Slots.Count < slotCount)
        {
            Slots.Add(new InventorySlot(data));
            SetIndex(Slots.Count - 1);
            OnSlotsChanged?.Invoke();
            return true;
        }

        return false;
    }

    // -------------------------
    // RemoveOne
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
        if (index >= 0 && index < Slots.Count)
        {
            CurrentIndex = index;
            NotifyChanged();
        }
    }

    public void Next()
    {
        if (Slots.Count == 0 || CurrentIndex < 0) return;

        CurrentIndex = (CurrentIndex + 1) % Slots.Count;
        NotifyChanged();
    }

    public void Prev()
    {
        if (Slots.Count == 0 || CurrentIndex < 0) return;

        CurrentIndex = (CurrentIndex - 1 + Slots.Count) % Slots.Count;
        NotifyChanged();
    }

    private void NotifyChanged()
    {
        OnCurrentItemChanged?.Invoke(CurrentItemData);
    }
}