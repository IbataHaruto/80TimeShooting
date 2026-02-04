using UnityEngine;

public class AnimalStatusManager : MonoBehaviour
{
    //動物の満腹度最大値
    public int maxFullness = 100;

    public void Feed(AnimalData animal, FruitsData fruit)
    {
        bool isFavorite = System.Array.Exists(animal.favoriteFruits, f => f == fruit);

        int gain = isFavorite ? fruit.score * 2 : fruit.score;

        animal.fullness += gain;

        if (isFavorite)
        {
            Debug.Log($"{animal.animalName} が 好物である {fruit.fruitName} を食べた → スコア2倍 +{gain}");
        }
        else
        {
            Debug.Log($"{animal.animalName} が {fruit.fruitName} を食べた → スコア +{gain}");
        }

        if (animal.fullness > maxFullness)
        {
            Debug.LogWarning($"{animal.animalName} の満腹度が最大値({maxFullness})を超えました → {animal.fullness}");
            animal.fullness = maxFullness;
        }

        // 現在のスコア（満腹度）を表示
        Debug.Log($"{animal.animalName} の現在の満腹度: {animal.fullness}/{maxFullness}");
    }
}