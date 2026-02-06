using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string volumeParameter;

    public void OnValueChanged(float value)
    {
        Debug.Log($"{volumeParameter} : {value}");
        float db = Mathf.Log10(value) * 20f;
        mixer.SetFloat(volumeParameter, db);
    }
}
