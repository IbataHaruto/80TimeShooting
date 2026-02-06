using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]float speed = 0.1f;
    [SerializeField] float gravity = -9.81f;     // 重力加速度
    [SerializeField] Enemy enemy;
    [SerializeField] Rigidbody rb;
    [SerializeField] CharacterController cCon;
    [SerializeField] float slopeLimit = 45f;     // 登れる最大傾斜角度
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        cCon.slopeLimit = slopeLimit; // 傾斜制限を設定
    }
    // Update is called once per frame
    void Update()
    {
        // 接地判定
        isGrounded = cCon.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 接地時に落下速度をリセット
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // カメラ方向に合わせた移動
        Vector3 move = transform.right * x + transform.forward * z;
        cCon.Move(move * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("GameScene");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            enemy.PlayerShift();
        }
        else
        {
            enemy.DontPlayerShift();
        }
        // 重力適用
        velocity.y += gravity * Time.deltaTime;
        cCon.Move(velocity * Time.deltaTime);
    }
}
