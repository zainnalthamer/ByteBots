using UnityEngine;
using TMPro;

public class BugPointsManager : MonoBehaviour
{
    public static BugPointsManager Instance;

    [SerializeField] TMP_Text counterText;

    private int maxValue;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        maxValue = GameObject.FindGameObjectsWithTag("Bug").Length;
        Refresh();
    }

    public void Refresh()
    {
        int value = SaveManager.I.GetBugCount();
        counterText.text = $"{value}/{maxValue}";
    }
}
