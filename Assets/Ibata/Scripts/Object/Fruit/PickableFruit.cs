using UnityEngine;

public class PickableFruit : MonoBehaviour, IInteractable
{
    public FruitsData data;

    public void Interact()
    {
        var player = FindObjectOfType<PlayerPickThrow>();

        if (player.Inventory.Add(data))
        {
            Destroy(gameObject);
            player.ExitCaptureMode();
        }
        else
        {
            player.ShowFullMessage();
        }
    }
}