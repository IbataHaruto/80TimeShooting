using UnityEngine;

[CreateAssetMenu(menuName = "Game/Fruit Data")]
public class FruitsData : ScriptableObject
{
    [Header("基本情報")]
    public string fruitName;
    public int score;

    [Header("インベントリ設定")]
    public int maxStack = 5;

    [Header("手持ち用プレハブ（UIモデル）")]
    public ThrowableObject handPrefab;

    [Header("ワールド用プレハブ（拾える & 投げられる）")]
    public GameObject pickablePrefab;

    [Header("UIアイコン")]
    public Sprite icon;
}