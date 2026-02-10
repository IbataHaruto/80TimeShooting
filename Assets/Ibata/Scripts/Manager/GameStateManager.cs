using UnityEngine;

public static class GameStateManager
{
    public static bool IsPaused { get; private set; }

    public static void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
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