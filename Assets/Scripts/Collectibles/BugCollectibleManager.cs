using TMPro;
using UnityEngine;

public class BugCollectibleManager : MonoBehaviour
{
    public static BugCollectibleManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text bugCounterText;

    private int bugsCaught = 0;
    private int totalBugs = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        totalBugs = FindObjectsOfType<BugInteraction>(true).Length;
        UpdateUI();
    }

    public void AddBugCaptured()
    {
        bugsCaught++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (bugCounterText)
            bugCounterText.text = $"Bugs: {bugsCaught}/{totalBugs}";
    }
}
