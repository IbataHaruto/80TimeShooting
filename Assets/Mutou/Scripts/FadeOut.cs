using System.Collections;
//using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UISystemProfilerApi;

public class FadeOut : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float durationSpeed = 0;

    private void Start()
    {
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        // Imageコンポーネントのカラーのアルファ値が1.0になるまで減算する
        while (image.color.a > 0.0f)
        {
            var color = image.color;
            color.a -= durationSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }

    }
}
