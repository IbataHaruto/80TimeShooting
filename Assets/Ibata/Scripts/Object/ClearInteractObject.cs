using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearInteractObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameGoalManager goalManager;
    [SerializeField] private string clearSceneName = "GameClear";

    public void Interact()
    {
        if (goalManager != null && goalManager.IsCleared)
        {
            SceneManager.LoadScene(clearSceneName);
        }
        else
        {
            Debug.Log("Clear conditions not met.");
        }
    }
}