using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject crosshair;

    void Start()
    {

        pauseUI.SetActive(false);
        crosshair.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool escPressed =
            Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame;

        bool startPressed =
            Gamepad.current != null &&
            Gamepad.current.startButton.wasPressedThisFrame;

        //  ESC Ç‹ÇΩÇÕ Gamepad Start Ç≈É|Å[ÉYêÿÇËë÷Ç¶
        if (escPressed || startPressed)
        {
            GameStateManager.TogglePause();

            pauseUI.SetActive(GameStateManager.IsPaused);
            crosshair.SetActive(!GameStateManager.IsPaused);
        }
    }
}