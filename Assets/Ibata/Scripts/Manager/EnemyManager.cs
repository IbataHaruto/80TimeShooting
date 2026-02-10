using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCrouch playerCrouch;

    [Header("Enemies")]
    [SerializeField] private Enemy[] enemies;   // © •¡”‚Ì“®•¨‚ð“o˜^

    void Start()
    {
        // ƒvƒŒƒCƒ„[‚Ì‚µ‚á‚ª‚Ýó‘Ô‚ª•Ï‚í‚Á‚½uŠÔ‚É‘S Enemy ‚É’Ê’m
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