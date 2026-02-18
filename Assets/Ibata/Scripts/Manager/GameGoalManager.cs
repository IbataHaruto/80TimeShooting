using UnityEngine;

public class GameGoalManager : MonoBehaviour
{
    [System.Serializable]
    public class Goal
    {
        public AnimalData animal;
        public int requiredCount;
    }

    public Goal[] goals;

    public bool IsCleared { get; private set; } = false;

    private AnimalStatusManager manager;

    private void Start()
    {
        manager = FindObjectOfType<AnimalStatusManager>();

        //  ScriptableObject の capturedCount をリセットしない（もう使わない）
        // 代わりに manager の Dictionary がプレイ中だけ管理する

        manager.OnCaptured += OnAnimalCaptured;
    }

    private void OnAnimalCaptured(AnimalData data)
    {
        if (IsCleared) return;

        if (CheckClear())
        {
            IsCleared = true;
            Debug.Log("ミッション達成！");
        }
    }

    private bool CheckClear()
    {
        foreach (var g in goals)
        {
            int current = manager.GetCapturedCount(g.animal);

            if (current < g.requiredCount)
                return false;
        }
        return true;
    }
}