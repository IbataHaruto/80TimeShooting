using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject pauseImage;
    [SerializeField] GameObject blome;
    bool isPaused = false;
    bool isRunning = false;
    [SerializeField] Transform target;
    [SerializeField] Transform target2;
    [SerializeField] Player player;


    void Update()
    {
        float dis = Vector3.Distance(target2.position, target.position);
        if(dis <= 5 && Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                ResumeGame();
                player.MouseCheck2();
            }
            else
            {
                PauseGame();
                player.MouseCheck();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isRunning)
            {
                StartGame();
                player.MouseCheck2();
            }
            else
            {
                StopGame();
                player.MouseCheck();
            }
        }

    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        image.SetActive(true);
        blome.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        image.SetActive(false);
        blome.SetActive(false);
    }
    public void StopGame()
    {
        Time.timeScale = 0f;
        isRunning = true;
        pauseImage.SetActive(true);
        blome.SetActive(true);
    }
    public void StartGame()
    {
        Time.timeScale = 0f;
        isRunning = false;
        pauseImage.SetActive(false);
        blome.SetActive(false);
    }
}
