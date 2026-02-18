using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private void Update()
    {
        if (IsInteractPressed())
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                var interact = hit.collider.GetComponent<ClearInteractObject>();
                if (interact != null)
                {
                    interact.Interact();
                }
            }
        }
    }

    private bool IsInteractPressed()
    {
        if (Input.GetKeyDown(KeyCode.E)) return true;
        if (Input.GetKeyDown(KeyCode.JoystickButton2)) return true;
        return false;
    }
}