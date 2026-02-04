using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    [SerializeField] Image image;
    [SerializeField] float durationSpeed = 0;

    public void StartFadeIn(string sceneName)
    {
        StartCoroutine(FadeCoroutine(sceneName));
    }

    public IEnumerator FadeCoroutine(string sceneName)
    {
        // Imageコンポーネントのカラーのアルファ値が1.0になるまで加算する
        while (image.color.a < 1.0f)
        {
            var color = image.color;
            color.a += durationSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }

        // アルファ値が1.0になったので、シーン切り替えをする
        SceneManager.LoadScene(sceneName);
    }


}
