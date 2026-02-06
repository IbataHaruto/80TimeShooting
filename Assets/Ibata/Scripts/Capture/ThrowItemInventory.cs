using UnityEngine;
using System;

public class ThrowItemInventory : MonoBehaviour
{
    [SerializeField] private int captureItemCount = 3;
    public int CaptureItemCount => captureItemCount;

    public event Action<int> OnCountChanged; //  UI ‚É’Ê’m

    public bool UseOne()
    {
        if (captureItemCount <= 0)
            return false;

        captureItemCount--;
        OnCountChanged?.Invoke(captureItemCount); //  ’Ê’m
        return true;
    }

    public void Add(int amount)
    {
        captureItemCount += amount;
        OnCountChanged?.Invoke(captureItemCount); //  ’Ê’m
    }
}