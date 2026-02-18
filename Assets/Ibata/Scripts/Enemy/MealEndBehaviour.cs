using UnityEngine;

public class MealEndBehaviour : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[MealEndBehaviour] OnStateExit 発火 → Meal アニメ終了");

        Enemy enemy = animator.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.EndMeal();
        }
        else
        {
            Debug.LogWarning("[MealEndBehaviour] Enemy が見つからない！");
        }
    }
}