using UnityEngine;

public class CaptureItemLifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 10f;
    private float timer;

    void Start()
    {
        timer = lifeTime;
    }

    void Update()
    {
        //  ポーズ中はカウントしない
        if (GameStateManager.IsPaused)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            Destroy(gameObject);
    }
}