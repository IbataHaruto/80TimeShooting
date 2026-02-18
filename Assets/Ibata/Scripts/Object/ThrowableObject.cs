using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [Header("Gravity Settings")]
    public float upwardGravity = -9.81f;
    public float downwardGravity = -30f; // ← オブジェクトごとに調整可能

    private bool isThrown = false;

    private Vector3 savedVelocity;
    private bool wasPaused = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.isKinematic = false;
        //rb.useGravity = false; // 独自重力
        col.enabled = true;
        isThrown = false;
    }

    public void Hold(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
        isThrown = false;
    }

    public void Throw(Vector3 force)
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = false;
        col.enabled = true;

        rb.AddForce(force, ForceMode.Impulse);
        isThrown = true;
    }

    void FixedUpdate()
    {
        if (GameStateManager.IsPaused)
        {
            if (!wasPaused)
            {
                savedVelocity = rb.linearVelocity;
                rb.isKinematic = true;
                wasPaused = true;
            }
            return;
        }

        if (wasPaused)
        {
            rb.isKinematic = false;
            rb.linearVelocity = savedVelocity;
            wasPaused = false;
        }

        if (!isThrown || rb.isKinematic)
            return;

        float g = rb.linearVelocity.y > 0 ? upwardGravity : downwardGravity;
        rb.AddForce(Vector3.up * g, ForceMode.Acceleration);
    }
}