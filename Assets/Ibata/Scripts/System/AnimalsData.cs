using UnityEngine;

[CreateAssetMenu(menuName = "Game/Animal Data")]
public class AnimalData : ScriptableObject
{
    public string animalName;

    //  好物を配列にすることで、2つでも3つでも自由に設定可能
    public FruitsData[] favoriteFruits;

    // ステータス(初期満腹度)
    public int fullness = 0;
}