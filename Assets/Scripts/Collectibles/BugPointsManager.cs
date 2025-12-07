using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BugPointsManager : MonoBehaviour
{
    public static BugPointsManager Instance;

    [SerializeField] TMP_Text counterText;
    [SerializeField] private int maxValue = 50;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        int value = SaveManager.I.GetBugCount();
        counterText.text = $"{value}/{maxValue}";
    }
}
