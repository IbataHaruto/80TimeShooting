using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;

    // UI の論理座標（全 CanvasScaler で統一）
    private readonly Vector2 referenceResolution = new Vector2(1920, 1080);

    private Resolution[] resolutions;

    void Start()
    {
        // --- UI の論理座標を全 CanvasScaler に適用 ---
        ApplyReferenceResolutionToAllCanvas();

        // --- 利用可能な解像度一覧を取得（width/height で distinct） ---
        resolutions = Screen.resolutions
            .Select(r => (r.width, r.height))
            .Distinct()
            .OrderByDescending(r => r.width)
            .Select(r => new Resolution { width = r.width, height = r.height })
            .ToArray();

        // --- Dropdown 初期化 ---
        dropdown.ClearOptions();

        var options = resolutions
            .Select(r => $"{r.width} x {r.height}")
            .ToList();

        dropdown.AddOptions(options);

        // 初期選択は referenceResolution と一致するもの
        int index = resolutions.ToList().FindIndex(r =>
            r.width == (int)referenceResolution.x &&
            r.height == (int)referenceResolution.y);

        dropdown.value = index >= 0 ? index : 0;
        dropdown.RefreshShownValue();

        dropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        var r = resolutions[index];

        // --- 実解像度のみ変更（UI の論理座標は固定） ---
        Screen.SetResolution(r.width, r.height, FullScreenMode.Windowed);
    }

    /// <summary>
    /// 全 CanvasScaler に referenceResolution を適用
    /// </summary>
    private void ApplyReferenceResolutionToAllCanvas()
    {
        foreach (var scaler in FindObjectsOfType<CanvasScaler>())
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = referenceResolution;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f; // 中間（必要に応じて調整）
        }
    }
}