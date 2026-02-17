using UnityEngine;

public class CaptureGoalUI : MonoBehaviour
{
    [SerializeField] private GameGoalManager goalManager;
    [SerializeField] private GoalEntryUI entryPrefab;
    [SerializeField] private Transform entryRoot;

    [SerializeField] private float verticalSpacing = -80f;

    private GoalEntryUI[] entries;
    private AnimalStatusManager manager;

    private void Start()
    {
        manager = FindObjectOfType<AnimalStatusManager>();

        var goals = goalManager.goals;
        entries = new GoalEntryUI[goals.Length];

        for (int i = 0; i < goals.Length; i++)
        {
            var g = goals[i];

            // ê∂ê¨
            var ui = Instantiate(entryPrefab, entryRoot);

            // à íuí≤êÆ
            RectTransform rt = ui.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(
                rt.anchoredPosition.x,
                i * verticalSpacing
            );

            //  manager ÇìnÇ∑ÇÊÇ§Ç…ïœçX
            ui.Initialize(manager, g.animal, g.requiredCount);

            entries[i] = ui;
        }

        manager.OnCaptured += OnCaptured;
    }

    private void OnCaptured(AnimalData data)
    {
        foreach (var e in entries)
        {
            e.UpdateCount();
        }
    }
}