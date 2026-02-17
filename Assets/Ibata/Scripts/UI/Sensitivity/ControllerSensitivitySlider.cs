using UnityEngine;
using UnityEngine.UI;

public class ControllerSensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public System.Action<float> OnChanged;

    void Start()
    {
        SensitivitySettings.Load();
        slider.value = SensitivitySettings.ControllerSensitivity;

        slider.onValueChanged.AddListener(value =>
        {
            OnChanged?.Invoke(value);
        });

        OnChanged?.Invoke(slider.value);
    }
}