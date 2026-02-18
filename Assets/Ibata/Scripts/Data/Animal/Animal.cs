using UnityEngine;
using System;

public class Animal : MonoBehaviour
{
    public AnimalData data;
    public AnimalStatusManager manager;

    public int currentFullness;
    public bool isCaptured = false;

    //  Meal 中かどうか（Enemy が制御）
    public bool isEating = false;

    public bool CanEat => !isCaptured && currentFullness < manager.maxFullness;

    public event Action OnEat;
    public event Action<bool> OnFullnessChanged;

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

            //  Meal 中でも OnEat は発火（満腹度は上がる）
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
                Destroy(gameObject);
            }

            Destroy(cap.gameObject);
        }
    }
}