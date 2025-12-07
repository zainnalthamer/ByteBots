using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public GameObject helpPanel;
    public AudioSource notEnoughSound;
    public int cost = 15;

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

        helpPanel.SetActive(true);
    }
}
