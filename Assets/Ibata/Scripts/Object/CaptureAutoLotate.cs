using UnityEngine;

public class CaptureAutoRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 2f; // ��]�̑����i�����j

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        // �� ���݂� up�i���Ă��鑤�j��
        // �� ���X�Ɂu������iVector3.up�j�v�֊񂹂�
        Vector3 currentUp = transform.up;
        Vector3 targetUp = Vector3.up;

        // Slerp �Ŋ��炩�ɕ��
        Vector3 newUp = Vector3.Slerp(currentUp, targetUp, rotateSpeed * Time.fixedDeltaTime);

        // up �� newUp �ɍ��킹�A�O�����͑��x�����Ɋ񂹂�
        Vector3 forward = rb.linearVelocity.normalized;
        if (forward.sqrMagnitude < 0.01f)
            forward = transform.forward;

        transform.rotation = Quaternion.LookRotation(forward, newUp);
    }
}