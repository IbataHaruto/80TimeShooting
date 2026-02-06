using UnityEngine;

public static class SensitivitySettings
{
    public static float MouseSensitivity = 1.0f;
    public static float ControllerSensitivity = 1.0f;
    public static float AdsSensitivityMultiplier = 0.5f;

    public static void Load()
    {
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        ControllerSensitivity = PlayerPrefs.GetFloat("ControllerSensitivity", 1.0f);
        AdsSensitivityMultiplier = PlayerPrefs.GetFloat("AdsSensitivityMultiplier", 0.5f);
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
        PlayerPrefs.SetFloat("ControllerSensitivity", ControllerSensitivity);
        PlayerPrefs.SetFloat("AdsSensitivityMultiplier", AdsSensitivityMultiplier);
    }
}