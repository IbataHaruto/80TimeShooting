using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCrouch playerCrouch;

    [Header("Enemies")]
    [SerializeField] private Enemy[] enemies;

    void Start()
    {
        playerCrouch.OnCrouchStateChanged += (isCrouching) =>
        {
            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                if (isCrouching)
                    enemy.PlayerCrouch();
                else
                    enemy.DontPlayerCrouch();
            }
        };
    }
}