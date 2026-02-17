using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] PauseManager pauseManager;
    public float moveSpeed = 5f;

    public GameObject firstButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }


    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null) return;

        Vector2 stickInput = gamepad.leftStick.ReadValue();
        Vector3 movement = new Vector3(stickInput.x, 0, stickInput.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetKeyDown("joystick button 0"))
        {
            pauseManager.PauseSwitch();
        }
    }
}
