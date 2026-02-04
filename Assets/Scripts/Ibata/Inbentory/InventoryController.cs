using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        // --- マウスホイールで切り替え ---
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0) inventory.Next();
            if (scroll < 0) inventory.Prev();
        }

        // --- 数字キーで直接選択 ---
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) inventory.SetIndex(0);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) inventory.SetIndex(1);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) inventory.SetIndex(2);
        }

        // --- ゲームパッド（十字キー） ---
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.right.wasPressedThisFrame) inventory.Next();
            if (Gamepad.current.dpad.left.wasPressedThisFrame) inventory.Prev();
        }
    }
}