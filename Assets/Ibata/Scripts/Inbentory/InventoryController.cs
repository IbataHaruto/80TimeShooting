using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private InventoryModel inventory;
    [SerializeField] private PlayerPickThrow pickThrow; //  追加

    void Update()
    {
        if (GameStateManager.IsPaused)
            return;

        // --- マウスホイールで切り替え ---
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0)
            {
                inventory.Next();
                pickThrow.ExitThrowItemMode(); //  捕獲モード終了
            }
            if (scroll < 0)
            {
                inventory.Prev();
                pickThrow.ExitThrowItemMode(); //  捕獲モード終了
            }
        }

        // --- 数字キーで直接選択 ---
        if (Keyboard.current != null)
        {
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                Key key = (Key)((int)Key.Digit1 + i);
                if (Keyboard.current[key].wasPressedThisFrame)
                {
                    inventory.SetIndex(i);
                    pickThrow.ExitThrowItemMode(); //  捕獲モード終了
                }
            }
        }

        // --- ゲームパッド（十字キー左右） ---
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.right.wasPressedThisFrame)
            {
                inventory.Next();
                pickThrow.ExitThrowItemMode(); //  捕獲モード終了
            }
            if (Gamepad.current.dpad.left.wasPressedThisFrame)
            {
                inventory.Prev();
                pickThrow.ExitThrowItemMode(); //  捕獲モード終了
            }
        }
    }
}