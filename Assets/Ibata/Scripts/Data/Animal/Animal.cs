using UnityEngine;

public class Animal : MonoBehaviour
{
    public AnimalData data;
    public AnimalStatusManager manager;

    public int currentFullness;
    public bool isCaptured = false;

    public bool CanEat => !isCaptured && currentFullness < manager.maxFullness;

    private void Start()
    {
        currentFullness = data.fullness;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCaptured) return;

        // --- 食べ物処理 ---
        Fruit fruit = other.GetComponent<Fruit>();
        if (fruit != null && CanEat)
        {
            manager.Feed(this, fruit.data);
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

                //  捕獲通知（カウントは manager が管理）
                manager.NotifyCaptured(data);

                Debug.Log($"{data.animalName} を捕獲しました！");
                Destroy(gameObject);
            }

            Destroy(cap.gameObject);
        }
    }
}