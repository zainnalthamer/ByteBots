using System.Collections.Generic;
using UnityEngine;

public class BugGroup : MonoBehaviour
{
    [Header("IDs & UI")]
    [SerializeField] private PuzzleID puzzleID;
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private bool countsTowardsTotal = true;

    [Header("Members")]
    [SerializeField] private List<BugAgent> bugs = new();

    public int TotalCount => bugs.Count;

    bool isDead;

    void Start()
    {
        if (puzzleID && SaveManager.I.IsPuzzleSolved(puzzleID.ID))
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isDead) return;

       // Time.timeScale = 0f;
        if (puzzleUI) puzzleUI.SetActive(true);
    }

    public void OnPuzzleSolved()
    {
        if (isDead) return;
        isDead = true;

        if (puzzleUI) puzzleUI.SetActive(false);
        Time.timeScale = 1f;

        if (puzzleID) SaveManager.I.MarkPuzzleSolved(puzzleID.ID);

        if (countsTowardsTotal)
        {
            SaveManager.I.IncrementBugCountBy(TotalCount);
            BugPointsManager.Instance.Refresh();
        }
        BugCollectibleManager.Instance?.Refresh();

        foreach (var b in bugs)
            if (b && b.gameObject.activeInHierarchy) b.Die();

        Invoke(nameof(DisableGroup), 4f);
    }

    void DisableGroup() => gameObject.SetActive(false);
}
