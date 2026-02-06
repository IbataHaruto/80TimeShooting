using UnityEngine;

public class PauseRigidbody : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
        {
            if (!rb.isKinematic)
            {
                savedVelocity = rb.linearVelocity;
                savedAngularVelocity = rb.angularVelocity;

                rb.isKinematic = true;
            }
        }
        else
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
                rb.linearVelocity = savedVelocity;
                rb.angularVelocity = savedAngularVelocity;
            }
        }
    }
}