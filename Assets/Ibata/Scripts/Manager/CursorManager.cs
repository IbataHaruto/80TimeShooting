using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    private bool cursorEnabled = false; // © ‰Šúó‘Ô‚Í”ñ•\¦•ƒƒbƒN

    void Start()
    {
        ApplyCursorState();
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.leftAltKey.wasPressedThisFrame)
        {
            cursorEnabled = !cursorEnabled;
            ApplyCursorState();
        }
    }

    private void ApplyCursorState()
    {
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