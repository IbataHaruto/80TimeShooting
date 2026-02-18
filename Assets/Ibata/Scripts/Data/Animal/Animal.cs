using UnityEngine;
using System;

public class Animal : MonoBehaviour
{
    public AnimalData data;
    public AnimalStatusManager manager;

    public int currentFullness;
    public bool isCaptured = false;

    public bool CanEat => !isCaptured && currentFullness < manager.maxFullness;

    // ★ イベント
    public event Action OnEat;
    public event Action<bool> OnFullnessChanged; // isMax を渡す

    private void Start()
    {
        currentFullness = data.fullness;
    }

    public void SetFullness(int value)
    {
        currentFullness = value;

        bool isMax = currentFullness >= manager.maxFullness;
        OnFullnessChanged?.Invoke(isMax);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCaptured) return;

        // --- 食べ物処理 ---
        Fruit fruit = other.GetComponent<Fruit>();
        if (fruit != null && CanEat)
        {
            manager.Feed(this, fruit.data);

            //  食べたイベント
            OnEat?.Invoke();

            Destroy(other.gameObject);
            return;
        }

        // --- 捕獲アイテム処理 ---
        CaptureItem cap = other.GetComponent<CaptureItem>();
        if (cap != null)
        {
            bool captured = manager.TryCapture(this);

            if (captured)
            {
                isCaptured = true;
                manager.NotifyCaptured(data);
                Debug.Log($"{data.animalName} を捕獲しました！");
                Destroy(gameObject);
            }

            Destroy(cap.gameObject);
        }
    }
}