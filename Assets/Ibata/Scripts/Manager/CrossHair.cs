using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Image crosshair;

    public void SetCanPick(bool canPick)
    {
        crosshair.color = canPick ? Color.red : Color.black;
    }
}