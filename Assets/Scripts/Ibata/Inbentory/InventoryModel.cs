using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public int CurrentIndex { get; private set; } = -1;

    public event Action<FruitsData> OnCurrentItemChanged;

    public InventorySlot CurrentSlot =>
        (CurrentIndex >= 0 && CurrentIndex < slots.Count)
        ? slots[CurrentIndex]
        : null;

    public FruitsData CurrentItemData =>
        CurrentSlot != null ? CurrentSlot.data : null;

    [SerializeField] private int slotCount = 3;
    public int SlotCount => slotCount;

    // -------------------------
    // Add
    // -------------------------
    public bool Add(FruitsData data)
    {
        // 1. 既存スロットに追加
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot.data == data && !slot.IsFull)
            {
                slot.AddOne();
                SetIndex(i);
                return true;
            }
        }

        // 2. 新規スロット
        if (slots.Count < slotCount)
        {
            slots.Add(new InventorySlot(data));
            SetIndex(slots.Count - 1);
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

        slot.RemoveOne();

        if (slot.Count == 0)
        {
            int removedIndex = CurrentIndex;
            slots.RemoveAt(removedIndex);

            // 削除後のインデックス調整
            if (slots.Count == 0)
            {
                CurrentIndex = -1;
            }
            else if (removedIndex < slots.Count)
            {
                CurrentIndex = removedIndex; // 次のスロットが繰り上がる
            }
            else
            {
                CurrentIndex = slots.Count - 1; // 最後のスロットへ
            }

            NotifyChanged();
        }
        else
        {
            NotifyChanged();
        }
    }

    // -------------------------
    // Index 操作
    // -------------------------
    public void SetIndex(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            CurrentIndex = index;
            NotifyChanged();
        }
    }

    public void Next()
    {
        if (slots.Count == 0 || CurrentIndex < 0) return;

        CurrentIndex = (CurrentIndex + 1) % slots.Count;
        NotifyChanged();
    }

    public void Prev()
    {
        if (slots.Count == 0 || CurrentIndex < 0) return;

        CurrentIndex = (CurrentIndex - 1 + slots.Count) % slots.Count;
        NotifyChanged();
    }

    // -------------------------
    // SelectFirstAvailable（削除後の選択位置調整のみ）
    // ※ RemoveAt は絶対に行わない
    // -------------------------
    public void SelectFirstAvailable()
    {
        if (slots.Count == 0)
        {
            CurrentIndex = -1;
        }
        else
        {
            CurrentIndex = 0;
        }

        NotifyChanged();
    }

    private void NotifyChanged()
    {
        OnCurrentItemChanged?.Invoke(CurrentItemData);
    }
}