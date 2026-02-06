using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    [SerializeField] private Transform target;   // 動物の Transform
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // ワールド座標 → UI の位置
        transform.position = target.position + offset;

        // カメラの方向を向く（常に正面）
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                         cam.transform.rotation * Vector3.up);
    }
}