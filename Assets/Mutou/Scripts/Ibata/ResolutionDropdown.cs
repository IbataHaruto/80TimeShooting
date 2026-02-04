using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private CanvasScaler canvasScaler;

    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions
            .Select(r => new Resolution { width = r.width, height = r.height })
            .Distinct()
            .OrderByDescending(r => r.width)
            .ToArray();

        dropdown.ClearOptions();

        var options = resolutions
            .Select(r => $"{r.width} x {r.height}")
            .ToList();

        dropdown.AddOptions(options);

        int currentIndex = resolutions.ToList().FindIndex(r =>
            r.width == Screen.currentResolution.width &&
            r.height == Screen.currentResolution.height);

        dropdown.value = currentIndex >= 0 ? currentIndex : 0;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        var r = resolutions[index];

        //  ウィンドウサイズを変更（Windowed を強制）
        Screen.SetResolution(r.width, r.height, FullScreenMode.Windowed);

        //  UI の論理サイズも変更（Canvas Scaler）
        if (canvasScaler != null)
        {
            canvasScaler.referenceResolution = new Vector2(r.width, r.height);
        }
    }
}