using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MissionInitilizeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("演出設定")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 2f;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void Show(string message)
    {
        missionText.text = message;
        gameObject.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        // フェードイン
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }

        yield return new WaitForSeconds(displayDuration);

        // フェードアウト
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}