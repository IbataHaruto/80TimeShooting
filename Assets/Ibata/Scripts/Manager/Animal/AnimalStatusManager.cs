using System.Collections.Generic;
using UnityEngine;

public class AnimalStatusManager : MonoBehaviour
{
    public int maxFullness = 100;

    public event System.Action<Animal, bool> OnFullnessChanged;
    public event System.Action<AnimalData> OnCaptured;

    //  動物ごとの捕獲数（プレイ中のみ）
    private Dictionary<AnimalData, int> capturedCounts = new();

    public int GetCapturedCount(AnimalData data)
    {
        if (capturedCounts.TryGetValue(data, out int count))
            return count;
        return 0;
    }

    public void Feed(Animal animal, FruitsData fruit)
    {
        bool isFavorite = animal.data.favoriteFruits != null &&
                          System.Array.Exists(animal.data.favoriteFruits, f => f == fruit);

        int gain = isFavorite ? fruit.score * 2 : fruit.score;

        animal.currentFullness = Mathf.Min(animal.currentFullness + gain, maxFullness);

        bool isMax = animal.currentFullness >= maxFullness;
        OnFullnessChanged?.Invoke(animal, isMax);
    }

    public bool TryCapture(Animal animal)
    {
        float rate = (float)animal.currentFullness / maxFullness;
        float rand = Random.value;
        return rand < rate;
    }

    //  捕獲成功時に呼ぶ（動物ごとのカウントを増やす）
    public void NotifyCaptured(AnimalData data)
    {
        if (!capturedCounts.ContainsKey(data))
            capturedCounts[data] = 0;

        capturedCounts[data]++;

        OnCaptured?.Invoke(data);
    }
}