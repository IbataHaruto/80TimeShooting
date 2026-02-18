using UnityEngine;

public class PlayerVisualController : MonoBehaviour
{
    [SerializeField] private StateObjects[] stateObjects;

    //  手オブジェクトをコードで指定
    [SerializeField] private GameObject handObject;

    private PlayerState currentState = PlayerState.Normal;
    private bool initialized = false;

    void Start()
    {
        InitializeState();
    }

    private void InitializeState()
    {
        foreach (var so in stateObjects)
        {
            bool active = (so.state == PlayerState.Normal);

            foreach (var obj in so.objects)
                obj.SetActive(active);
        }

        //  初期状態は Normal → 手を表示
        if (handObject != null)
            handObject.SetActive(true);

        initialized = true;
    }

    public void SetState(PlayerState newState)
    {
        if (!initialized)
            return;

        if (newState == currentState)
            return;

        currentState = newState;

        // --- 状態ごとのオブジェクト切り替え ---
        foreach (var so in stateObjects)
        {
            bool active = (so.state == currentState);

            foreach (var obj in so.objects)
                obj.SetActive(active);
        }

        // ---  しゃがみなら手を非表示、それ以外は表示 ---
        if (handObject != null)
        {
            bool showHand = (currentState != PlayerState.Crouch);
            handObject.SetActive(showHand);
        }
    }
}