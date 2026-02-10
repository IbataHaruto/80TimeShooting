using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManajer : MonoBehaviour
{
    public Image fadeImage;  // フェード用画像
    [SerializeField] float fadeDuration = 1f;  // フェードの時間
    private bool isTransitioning = false;
    private static GameObject mainCanvasInstance;  // MainのCanvasのインスタンス
    [SerializeField] float FadeSpeed;
    public GameObject[] mainUI;

    void Awake()
    {

        // MainシーンのCanvasのみDontDestroyOnLoadにする
        if (SceneManager.GetActiveScene().name == "SampleScene" && mainCanvasInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            mainCanvasInstance = gameObject;  // このオブジェクトを保存
        }
    }

    void Start()
    {
        //// 最初は透明にする（Alphaを0）
        //fadeImage.color = new Color(0, 0, 0, 0);

    }

    public void StartSceneTransition(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(SwitchScene(sceneName));
        }
    }

    // フェードアウトしてシーンを切り替える
    private IEnumerator SwitchScene(string sceneName)
    {
        isTransitioning = true;

        // フェードイン
        yield return StartCoroutine(Fade(1));

        // シーンを切り替えとUI要素削除
        SceneManager.LoadScene(sceneName);
        foreach (GameObject uiElement in mainUI)
        {
            uiElement.SetActive(false);
        }

        // シーンが切り替わるまで待つ（次のフレーム）
        yield return null;

        // フェードアウト
        yield return StartCoroutine(Fade(0));

        isTransitioning = false;
    }

    // フェード処理
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0;

        if (targetAlpha == 1)
        {
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

        }
        if (targetAlpha == 0)
        {
            while (time < FadeSpeed)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / FadeSpeed);
                fadeImage.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

}
