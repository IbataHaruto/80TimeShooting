using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void StartGame()
    {
        //StartCoroutine(fadeIn.FadeCoroutine("New Scene"));
        SceneManager.LoadScene("InGameScene");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // エディタ再生停止
#else
    Application.Quit(); // ビルド後アプリを終了
#endif
        }

    }

}
