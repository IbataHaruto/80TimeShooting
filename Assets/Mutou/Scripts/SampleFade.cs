using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SampleFade : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float durationSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FadeSample());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public IEnumerator FadeSample()
    {
        // Imageコンポーネントのカラーのアルファ値が1.0になるまで加算する
        while (image.color.a > 0.0f)
        {
            var color = image.color;
            color.a -= durationSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }

    }
}
