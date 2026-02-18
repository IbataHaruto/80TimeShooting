using UnityEngine;

public class FruitPickup : MonoBehaviour, IInteractable
{
    public FruitsData data;

    public void Interact()
    {
        var player = FindObjectOfType<PlayerPickThrow>();

        if (player.Inventory.Add(data))
        {
            player.ExitCaptureMode();
        }
        else
        {
            player.ShowFullMessage();
        }
    }
}