using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private PlayerPickThrow pickThrow;

    private void Start()
    {
        inventory.SetIndex(0);
        pickThrow.ExitCaptureMode(); // 念のため捕獲モードも OFF にしておく
    }

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        int before = inventory.CurrentIndex;

        // ============================
        // マウスホイール（ループする）
        // ============================
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll > 0)
            {
                inventory.Next();   // ← ループする
            }
            if (scroll < 0)
            {
                inventory.Prev();   // ← ループする
            }
        }

        // ============================
        // 数字キー（1~4のみ有効）
        // ============================
        if (Keyboard.current != null)
        {
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                Key key = (Key)((int)Key.Digit1 + i);
                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    inventory.SetIndex(i);
                }
            }
            // 5以上は無視
        }

        // ============================
        // D-pad（ループする）
        // ============================
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
                inventory.Next();

            if (Gamepad.current.dpad.left.wasPressedThisFrame)
                inventory.Prev();
        }

        // ============================
        // スロット変更後の捕獲モード更新
        // ============================
        int after = inventory.CurrentIndex;

        if (after != before)
        {
            if (inventory.IsCaptureSlot(after))
                pickThrow.EnterCaptureMode();
            else
                pickThrow.ExitCaptureMode();
        }
    }
}