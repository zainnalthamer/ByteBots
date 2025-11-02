using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
    private bool once;
    private void OnTriggerEnter(Collider other)
    {
        if (once || !other.CompareTag("Player")) return;
        once = true;
        SaveManager.I.MarkLevelCompleted(); // current scene
        // TODO: open victory UI or load next scene
    }
}
