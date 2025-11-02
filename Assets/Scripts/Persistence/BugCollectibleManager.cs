using TMPro;
using UnityEngine;

public class BugCollectibleManager : MonoBehaviour
{
    public static BugCollectibleManager Instance { get; private set; }
    [SerializeField] private TMP_Text bugCounterText;

    void Awake() => Instance = this;
    void Start() => Refresh();

    public void Refresh()
    {
        if (bugCounterText)
            bugCounterText.text = $"Bugs: {SaveManager.I.GetBugCount()}";
    }
}
