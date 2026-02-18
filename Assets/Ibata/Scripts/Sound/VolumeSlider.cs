using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager.Instance が存在しません");
            return;
        }

        // 保存された値をスライダーに反映
        masterSlider.value = SoundManager.Instance.masterVolume;
        bgmSlider.value = SoundManager.Instance.bgmVolume;
        seSlider.value = SoundManager.Instance.seVolume;
    }

    public void OnMasterChanged(float v)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.masterVolume = v;
        SoundManager.Instance.SaveVolumes();
    }

    public void OnBGMChanged(float v)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.bgmVolume = v;
        SoundManager.Instance.SaveVolumes();
    }

    public void OnSEChanged(float v)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.seVolume = v;
        SoundManager.Instance.SaveVolumes();
    }
}