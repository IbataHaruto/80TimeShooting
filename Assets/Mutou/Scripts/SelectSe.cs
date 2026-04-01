using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSe : MonoBehaviour
{
    public static SelectSe Instance;

    [SerializeField] AudioSource seSource;
    [SerializeField] AudioClip selectSe;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySelect()
    {
        seSource.PlayOneShot(selectSe);
    }
}
