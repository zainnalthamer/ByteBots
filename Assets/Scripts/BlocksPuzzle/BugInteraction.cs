using UnityEngine;

public class BugInteraction : MonoBehaviour
{
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private Animator bugAnimator;
    [SerializeField] private float deathDelay = 4f;
    private bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            if (puzzleUI != null)
                puzzleUI.SetActive(true);
        }
    }

    public void OnPuzzleSolved()
    {
        if (isDead) return;
        isDead = true;

        Time.timeScale = 1f;

        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (bugAnimator != null)
            bugAnimator.SetTrigger("Die");

        Invoke(nameof(DisableBug), deathDelay);
    }

    private void DisableBug()
    {
        gameObject.SetActive(false);
    }
}
