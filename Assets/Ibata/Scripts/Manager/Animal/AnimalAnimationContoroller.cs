using UnityEngine;

public class AnimalAnimationController : MonoBehaviour
{
    private Animator animator;
    private Animal animal;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animal = GetComponent<Animal>();
    }

    void Start()
    {
        //  StatusManager のイベント購読
        FindObjectOfType<AnimalStatusManager>().OnFullnessChanged += OnFullnessChanged;
    }

    //  満腹度が最大になったらアニメーション切り替え
    void OnFullnessChanged(Animal changedAnimal, bool isMax)
    {
        if (changedAnimal != animal) return;

        if (isMax)
        {
            animator.SetTrigger("Sleep");   // 満腹アニメーションへ
        }
        else
        {
            animator.ResetTrigger("Sleep");
        }
    }
}