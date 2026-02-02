using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        Changescene();
    }
    void Changescene()
    {
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene("New Scene");
        }
        
    }

}
