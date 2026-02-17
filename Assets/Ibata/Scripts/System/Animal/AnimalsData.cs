using UnityEngine;

[CreateAssetMenu(menuName = "Game/Animal Data")]
public class AnimalData : ScriptableObject
{
    public string animalName;
    public FruitsData[] favoriteFruits;
    public int fullness = 0;

    public Sprite icon;

    [TextArea]
    public string description;
}