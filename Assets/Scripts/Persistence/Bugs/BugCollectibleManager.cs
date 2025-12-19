using TMPro;
using UnityEngine;
using System.Linq;

public class BugCollectibleManager : MonoBehaviour
{
    public static BugCollectibleManager Instance { get; private set; }
    [SerializeField] private TMP_Text bugCounterText;

    void Awake() => Instance = this;
    void Start() => Refresh();

    public void Refresh()
    {
        int caught = SaveManager.I.GetBugCount();
        int total = CalculateTotalBugsInScene();
        if (bugCounterText)
            bugCounterText.text = $"Bugs: {caught} / {total}";
    }

    int CalculateTotalBugsInScene()
    {
        var allGroups = Resources.FindObjectsOfTypeAll<BugGroup>();
        int groupTotal = allGroups.Where(g => g != null).Sum(g => g.TotalCount);

        var singles = Resources.FindObjectsOfTypeAll<BugInteraction>();
        int singleTotal = singles.Count(s => s != null);

        return groupTotal + singleTotal;
    }
}