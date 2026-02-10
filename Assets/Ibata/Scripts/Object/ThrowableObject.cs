using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [Header("Gravity Settings")]
    [SerializeField] private float upwardGravity = -9.81f;   // 上昇中の重力（弱め）
    [SerializeField] private float downwardGravity = -30f;   // 下降中の重力（強め）

    private bool isThrown = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        // ワールドに置かれた状態
        rb.isKinematic = false;
        rb.useGravity = true; // 独自重力を使う
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
        if (!isThrown || rb.isKinematic)
            return;

        float g = rb.linearVelocity.y > 0 ? upwardGravity : downwardGravity;

        rb.AddForce(Vector3.up * g, ForceMode.Acceleration);
    }
}