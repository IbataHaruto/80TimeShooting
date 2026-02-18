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

    public MissionInitilizeUI missionIntroUI;


    public bool IsCleared { get; private set; } = false;

    private AnimalStatusManager manager;


    //  ミッション開始演出 UI（新規）
    public MissionInitilizeUI missionInnitialUI;

    private void Start()
    {
        manager = FindObjectOfType<AnimalStatusManager>();
        manager.OnCaptured += OnAnimalCaptured;

        ShowMissionIntro();
    }

    private void ShowMissionIntro()
    {
        string msg = "ミッション\n";

        foreach (var g in goals)
        {
            string animal = $"<color=#FFCC00>{g.animal.animalName}</color>";
            string count = $"<color=#FF7F50>{g.requiredCount}</color>";

            msg += $"{animal} を {count} ひき捕まえろ\n";
        }

        missionIntroUI.Show(msg);
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

    // UI が必要なら呼べる API
    public Goal[] GetGoals() => goals;
}