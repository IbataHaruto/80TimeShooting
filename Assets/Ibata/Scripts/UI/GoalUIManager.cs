//using UnityEngine;

//public class GoalUIManager : MonoBehaviour
//{
//    [SerializeField] private GoalEntryUI entryPrefab;
//    [SerializeField] private Transform parent;

//    private GoalEntryUI[] entries;

//    public void InitializeGoals(GameGoalManager.Goal[] goals)
//    {
//        entries = new GoalEntryUI[goals.Length];

//        for (int i = 0; i < goals.Length; i++)
//        {
//            var g = goals[i];

//            var entry = Instantiate(entryPrefab, parent);
//            entry.Initialize(
//                FindObjectOfType<AnimalStatusManager>(),
//                g.animal,
//                g.requiredCount
//            );

//            entries[i] = entry;
//        }
//    }

//    public void UpdateAll()
//    {
//        if (entries == null) return;

//        foreach (var e in entries)
//        {
//            e.UpdateCount();
//        }
//    }
//}