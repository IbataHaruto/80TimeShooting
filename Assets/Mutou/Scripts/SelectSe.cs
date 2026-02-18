using UnityEngine;
using UnityEngine.EventSystems;

public class SelectSe : MonoBehaviour
{
    [SerializeField] AudioSource seSource;
    [SerializeField] AudioClip selectSe;

    public void OnSelect(BaseEventData eventData)
    {
        // ボタン選択中にSEを鳴らす
        seSource.PlayOneShot(selectSe);
    }
}
