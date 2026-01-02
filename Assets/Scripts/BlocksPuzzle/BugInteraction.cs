using UnityEngine;

public class OldBugInteraction : MonoBehaviour
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
            //BugCollectibleManager.Instance.AddBugCaptured();
        }

        if (puzzleUI) puzzleUI.SetActive(false); 

        if (bugAnimator) bugAnimator.SetTrigger("Die");
        KillBug();
    }

    private void KillBug()
    {
        Destroy(gameObject, deathDelay);
    }
}
