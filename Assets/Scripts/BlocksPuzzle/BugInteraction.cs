using UnityEngine;

public class BugInteraction : MonoBehaviour
{
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private Animator bugAnimator;
    [SerializeField] private float deathDelay = 3f;

    private bool isDead = false;
    private bool counted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            if (puzzleUI) puzzleUI.SetActive(true);
        }
    }

    public void OnPuzzleSolved()
    {
        if (isDead) return;
        isDead = true;

        if (!counted && BugCollectibleManager.Instance != null)
        {
            counted = true;
            BugCollectibleManager.Instance.AddBugCaptured();
        }

        if (puzzleUI) puzzleUI.SetActive(false);
        Time.timeScale = 1f;

        if (bugAnimator) bugAnimator.SetTrigger("Die");
        Invoke(nameof(DisableBug), deathDelay);
    }

    private void DisableBug()
    {
        gameObject.SetActive(false);
    }
}
