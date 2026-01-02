using UnityEngine;

public class PuzzleID : MonoBehaviour
{
    [SerializeField] private string id;   // e.g., "carnival_bug_01"
    public string ID => id;
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id))
            Debug.LogWarning($"[PuzzleID] Please set a stable ID on {name}.");
    }
#endif
}
