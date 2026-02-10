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
        // --- 食べ物処理 ---
        if (CanEat)
        {
            Fruit fruit = other.GetComponent<Fruit>();
            if (fruit != null)
            {
                manager.Feed(this, fruit.data);
                Destroy(other.gameObject);
                return;
            }
        }

        // --- 捕獲アイテム処理 ---
        CaptureItem cap = other.GetComponent<CaptureItem>();
        if (cap != null)
        {
            bool captured = manager.TryCapture(this);

            if (captured)
            {
                isCaptured = true;
                Debug.Log($"{data.animalName} を捕獲しました！");
                Destroy(gameObject);
            }

            Destroy(cap.gameObject);
        }
    }
}