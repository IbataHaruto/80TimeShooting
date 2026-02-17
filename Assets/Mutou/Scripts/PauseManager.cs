using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject pauseImage;
    [SerializeField] GameObject blome;
    [SerializeField] GameObject miniMap;
    bool isPaused = false;
    bool isRunning = false;
    [SerializeField] Transform target;
    [SerializeField] Transform target2;
    [SerializeField] Player player;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PauseSwitch();
        }
    }
    public void PauseSwitch()
    {
        float dis = Vector3.Distance(target2.position, target.position);
        if (dis <= 5 )
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
            if (isRunning)
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
        miniMap.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        image.SetActive(false);
        blome.SetActive(false);
        miniMap.SetActive(true);
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
        Time.timeScale = 1f;
        isRunning = false;
        pauseImage.SetActive(false);
        blome.SetActive(false);
    }
}
