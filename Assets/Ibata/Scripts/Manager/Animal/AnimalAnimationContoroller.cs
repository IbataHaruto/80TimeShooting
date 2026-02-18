using UnityEngine;

public class AnimalAnimationController : MonoBehaviour
{
    private Animator animator;
    private Animal animal;
    private AnimalStatusManager manager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animal = GetComponent<Animal>();
        manager = animal.manager; // FindObjectOfType を排除
    }

    void Start()
    {
        //  Animal のイベント購読
        animal.OnEat += PlayMeal;
        animal.OnFullnessChanged += OnFullnessChanged;

        //  StatusManager のイベント購読（Sleep 用）
        manager.OnFullnessChanged += OnManagerFullnessChanged;
    }

    // --- 食事アニメーション ---
    void PlayMeal()
    {
        if (animal.isEating) return; //  Meal 中はアニメーション更新しない

        animator.SetTrigger("Meal");
    }

    // --- Animal 側の満腹イベント（内部状態変化） ---
    void OnFullnessChanged(bool isMax)
    {
        if (isMax)
        {
            animator.SetTrigger("Sleep");
        }
    }

    // --- Manager 側の満腹イベント（Animal 識別付き） ---
    void OnManagerFullnessChanged(Animal changedAnimal, bool isMax)
    {
        if (changedAnimal != animal) return;

        if (isMax)
        {
            animator.SetTrigger("Sleep");
        }
    }
}