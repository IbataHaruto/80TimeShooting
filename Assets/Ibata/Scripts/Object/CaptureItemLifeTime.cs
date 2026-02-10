using UnityEngine;

public class CaptureItemLifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}