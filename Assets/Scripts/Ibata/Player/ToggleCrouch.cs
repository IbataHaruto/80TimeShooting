using UnityEngine;
using UnityEngine.UI;

public class CrouchModeSwitcher : MonoBehaviour
{
    [SerializeField] private PlayerCrouch playerCrouch;
    [SerializeField] private Toggle crouchToggle;
    // ON = Hold, OFF = Toggle にする例

    private bool pendingModeIsHold; // UI の一時状態

    void Start()
    {
        // 初期状態を UI に反映
        bool isHold = playerCrouch.crouchMode == PlayerCrouch.CrouchMode.Hold;
        crouchToggle.isOn = isHold;

        // UI の変更は「一時的に保存」だけ
        pendingModeIsHold = crouchToggle.isOn;

        crouchToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    // UI Toggle が変更されたら「一時保存」
    void OnToggleChanged(bool isOn)
    {
        pendingModeIsHold = isOn;
    }

    //  適用ボタンから呼ばれる
    public void Apply()
    {
        if (pendingModeIsHold)
            playerCrouch.crouchMode = PlayerCrouch.CrouchMode.Hold;
        else
            playerCrouch.crouchMode = PlayerCrouch.CrouchMode.Toggle;

        Debug.Log("Crouch mode applied: " + playerCrouch.crouchMode);
    }
}