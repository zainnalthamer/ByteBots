using UnityEngine;

public class BugInteraction : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private Animator bugAnimator;
    [SerializeField] private PuzzleID puzzleID;
    [SerializeField] private float deathDelay = 3f;

    private bool isDead = false;

    private void Start()
    {
        if (puzzleID && SaveManager.I.IsPuzzleSolved(puzzleID.ID))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (!other.CompareTag("Player")) return;

        Time.timeScale = 0f;
        if (puzzleUI) puzzleUI.SetActive(true);
    }

    public void OnPuzzleSolved()
    {
        if (isDead) return;
        isDead = true;

        if (puzzleID) SaveManager.I.MarkPuzzleSolved(puzzleID.ID);
        SaveManager.I.IncrementBugCount();
        BugCollectibleManager.Instance?.Refresh();

        if (puzzleUI) puzzleUI.SetActive(false);
        Time.timeScale = 1f;

        if (bugAnimator) bugAnimator.SetTrigger("Die");
        Invoke(nameof(DisableBug), deathDelay);
    }

    private void DisableBug() => gameObject.SetActive(false);
}