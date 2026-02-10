using UnityEngine;

public class InventorySlot
{
    public FruitsData Data { get; private set; }
    public int Count { get; private set; }

    public int MaxStack => Data.maxStack;
    public bool IsFull => Count >= MaxStack;
    public bool IsEmpty => Count <= 0;

    public InventorySlot(FruitsData data, int initialCount = 1)
    {
        Data = data;
        Count = Mathf.Clamp(initialCount, 0, data.maxStack);
    }

    public bool CanAddOne() => Count < MaxStack;
    public bool CanRemoveOne() => Count > 0;

    public bool AddOne()
    {
        if (!CanAddOne()) return false;
        Count++;
        return true;
    }

    public bool RemoveOne()
    {
        if (!CanRemoveOne()) return false;
        Count--;
        return true;
    }
}