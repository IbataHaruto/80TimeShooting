using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    private bool cursorEnabled = false; // Alt で切り替える状態

    void Start()
    {
        ApplyCursorState();
    }

    void Update()
    {
        // ポーズ中は常にカーソル表示
        if (GameStateManager.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // Alt でカーソル切り替え
        if (Keyboard.current != null &&
            Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            cursorEnabled = !cursorEnabled;
            ApplyCursorState();
        }

        //  ロック解除中にクリックしたらロック復帰
        if (cursorEnabled &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            cursorEnabled = false;
            ApplyCursorState();
        }
    }

    private void ApplyCursorState()
    {
        if (GameStateManager.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if (cursorEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}