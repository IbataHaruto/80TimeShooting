using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Transform player;       // プレイヤーのTransform
    public float escapeDistance = 10f; // この距離以内に入ると逃げる
    public float moveSpeed = 5f;       // 移動速度
    public float heightOffset = 3f;    // 逃げるときの上昇量
    public float smoothRotation = 5f;  // 回転のスムーズさ
    private bool flyFlag = false;
    private bool shiftFlag = false;
    private Vector3 targetPosition;

    void Update()
    {
        // プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < escapeDistance && !shiftFlag)
        {
            flyFlag = true;
        }
        if (flyFlag)
        {
            // プレイヤーの反対方向に逃げる位置を計算
            Vector3 directionAway = (transform.position - player.position).normalized;

            // 上方向にも逃げる
            directionAway.y += heightOffset / escapeDistance;

            // 逃げ先のターゲット位置
            targetPosition = transform.position + directionAway * moveSpeed * Time.deltaTime;

            // 回転をプレイヤーの反対方向に向ける
            Quaternion lookRotation = Quaternion.LookRotation(directionAway);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothRotation);

            // 移動
            transform.position += transform.forward * moveSpeed;

            Debug.Log("Fly");
        }
        else
        {
            Debug.Log("Not Fly");
        }
        if (transform.position.y >= 15)
        {
            Destroy(gameObject);
            Debug.Log("Not Object");
        }
    }
    public void ShiftObject()
    {
        shiftFlag = true;
    }
    public void DontShiftObject()
    {
        shiftFlag = false;
    }
}
