public class InventorySlot
{
    public FruitsData data;
    public int Count { get; private set; }

    public int MaxStack => data.maxStack;
    public bool IsFull => Count >= MaxStack;

    public InventorySlot(FruitsData data)
    {
        this.data = data;
        Count = 1;
    }

    public void AddOne() => Count++;
    public void RemoveOne() => Count--;
}