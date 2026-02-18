using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }

    public PlayerState CurrentState { get; private set; } = PlayerState.Normal;

    [SerializeField] private PlayerVisualController visualController;

    void Awake()
    {
        Instance = this;
    }

    public void SetState(PlayerState newState)
    {
        if (newState == CurrentState)
            return;

        CurrentState = newState;
        visualController.SetState(newState);
    }
}