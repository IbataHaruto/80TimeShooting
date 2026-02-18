using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;

    [Header("Gravity Settings")]
    [SerializeField] private float upwardGravity = -9.81f;
    [SerializeField] private float downwardGravity = -30f;

    private bool isThrown = false;

    // ★ ポーズ中の速度保存用
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
        //  ポーズ中の処理
        if (GameStateManager.IsPaused)
        {
            if (!wasPaused)
            {
                // 初回だけ保存
                savedVelocity = rb.linearVelocity;
                rb.isKinematic = true;   // 完全停止
                wasPaused = true;
            }
            return;
        }

        //  ポーズ解除時の処理
        if (wasPaused)
        {
            rb.isKinematic = false;       // 物理再開
            rb.linearVelocity = savedVelocity; // 速度復元
            wasPaused = false;
        }

        if (!isThrown || rb.isKinematic)
            return;

        // 独自重力
        float g = rb.linearVelocity.y > 0 ? upwardGravity : downwardGravity;
        rb.AddForce(Vector3.up * g, ForceMode.Acceleration);
    }
}