using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        //  保存された値をスライダーに反映
        masterSlider.value = SoundManager.Instance.masterVolume;
        bgmSlider.value = SoundManager.Instance.bgmVolume;
        seSlider.value = SoundManager.Instance.seVolume;
    }

    public void OnMasterChanged(float v)
    {
        SoundManager.Instance.masterVolume = v;
        SoundManager.Instance.SaveVolumes();
    }

    public void OnBGMChanged(float v)
    {
        SoundManager.Instance.bgmVolume = v;
        SoundManager.Instance.SaveVolumes();
    }

    public void OnSEChanged(float v)
    {
        SoundManager.Instance.seVolume = v;
        SoundManager.Instance.SaveVolumes();
    }
}