using UnityEngine;

public class CaptureItem : MonoBehaviour
{
    private void Awake()
    {
        // ”O‚Ì‚½‚ß Collider ‚ð Trigger ‚É‚µ‚Ä‚¨‚­
        var col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }
}