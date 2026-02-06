using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] GameObject blome;
    bool isPaused = false;
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
}
