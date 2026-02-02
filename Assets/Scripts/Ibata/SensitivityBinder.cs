using UnityEngine;

public class SensitivityBinder : MonoBehaviour
{
    [SerializeField] private MouseSensitivitySlider mouseSlider;
    [SerializeField] private ControllerSensitivitySlider controllerSlider;
    [SerializeField] private ADSSensitivitySlider adsSlider;

    void Start()
    {
        mouseSlider.OnChanged += value =>
        {
            SensitivitySettings.MouseSensitivity = value;
            SensitivitySettings.Save();
        };

        controllerSlider.OnChanged += value =>
        {
            SensitivitySettings.ControllerSensitivity = value;
            SensitivitySettings.Save();
        };

        adsSlider.OnChanged += value =>
        {
            SensitivitySettings.AdsSensitivityMultiplier = value;
            SensitivitySettings.Save();
        };
    }
}