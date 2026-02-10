using System;
using UnityEngine;

public class AnimalStatusManager : MonoBehaviour
{
    public int maxFullness = 100;

    //  満腹度が変わった時に「最大かどうか」も通知する
    public event Action<Animal, bool> OnFullnessChanged;

    public void Feed(Animal animal, FruitsData fruit)
    {
        bool isFavorite = Array.Exists(animal.data.favoriteFruits, f => f == fruit);
        int gain = isFavorite ? fruit.score * 2 : fruit.score;

        animal.currentFullness += gain;
        if (animal.currentFullness > maxFullness)
            animal.currentFullness = maxFullness;

        bool isMax = animal.currentFullness >= maxFullness;

        Debug.Log($"{animal.data.animalName} の現在の満腹度: {animal.currentFullness}/{maxFullness}");

        //  満腹度が最大かどうかをアニメーション側に通知
        OnFullnessChanged?.Invoke(animal, isMax);
    }

    // 捕獲判定
    public bool TryCapture(Animal animal)
    {
        float rate = (float)animal.currentFullness / maxFullness;
        float rand = UnityEngine.Random.value;

        bool success = rand < rate;

        Debug.Log(
            $"{animal.data.animalName} 捕獲判定 → 捕獲率:{rate * 100:F1}% / 抽選値:{rand:F2} → {(success ? "成功" : "失敗")}"
        );

        return success;
    }
}