using UnityEngine;

[CreateAssetMenu(menuName = "Game/Fruit Data")]
public class FruitsData : ScriptableObject
{
    [Header("基本情報")]
    public string fruitName;
    public int score;

    [Header("インベントリ設定")]
    public int maxStack = 5;

    [Header("手持ち用プレハブ（非実体モデル）")]
    public ThrowableFruit handPrefab;

    [Header("投げる用プレハブ（ワールド実体）")]
    public ThrowableFruit worldPrefab;
}