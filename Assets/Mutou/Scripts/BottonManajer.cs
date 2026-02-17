using UnityEngine;
using UnityEngine.SceneManagement;

public class BottonManajer : MonoBehaviour
{
    public void SampleBotton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void SampleBotton2()
    {
       Debug.Log("Animal");
    }

}
