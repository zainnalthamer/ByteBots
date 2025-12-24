using UnityEngine;
using TMPro;

public class HelpButton : MonoBehaviour
{
    public static string CurrentConcept = "Concept";

    public GameObject helpPanel;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private TextMeshProUGUI examplesText;

    public AudioSource notEnoughSound;
    public int cost = 0;

    public void OnHelpClicked()
    {
        int current = SaveManager.I.GetBugCount();

        if (current < cost)
        {
            if (notEnoughSound) notEnoughSound.Play();
            return;
        }

        SaveManager.I.IncrementBugCountBy(-cost);
        BugPointsManager.Instance.Refresh();

        explainText.text = $"Explain {CurrentConcept}";
        examplesText.text = $"Examples of {CurrentConcept}";

        helpPanel.SetActive(true);
    }
}
