using UnityEngine;
using TMPro;

public class BugPointsManager : MonoBehaviour
{
    public static BugPointsManager Instance;

    [SerializeField] TMP_Text counterText;

    private int maxValue = 50;

    void Awake()
    {
        Instance = this;
        Refresh();
    }
     
    public void Refresh()
    {
        int value = SaveManager.I.GetBugCount();
        counterText.text = $"{value}/{maxValue}";
    }
}
