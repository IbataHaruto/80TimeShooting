using UnityEngine;

public class ThrowableFruit : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();   //  修正ポイント

        if (rb != null) rb.isKinematic = true;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void Hold(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void Throw(Vector3 force)
    {
        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        //  Collider を有効化する
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = true;

        rb.AddForce(force, ForceMode.Impulse);
    }
}