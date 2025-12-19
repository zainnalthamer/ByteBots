using UnityEngine;

public class BookmarkController : MonoBehaviour
{
    [Header("Main UI Parents")]
    public GameObject puzzlePanel;
    public GameObject helpPanel;
    public GameObject learnPanel;

    [Header("Help UI Children")]
    public GameObject choiceCardUI;
    public GameObject lumaChatUI;

    private void Start()
    {
        puzzlePanel.SetActive(true);
        helpPanel.SetActive(false);
        learnPanel.SetActive(false);

        choiceCardUI.SetActive(false);
        lumaChatUI.SetActive(false);
    }

    public void OnPuzzleBookmarkClick()
    {
        puzzlePanel.SetActive(true);
        helpPanel.SetActive(false);
        learnPanel.SetActive(false);

        choiceCardUI.SetActive(false);
        lumaChatUI.SetActive(false);
    }

    public void OnHelpBookmarkClick()
    {
        puzzlePanel.SetActive(false);
        learnPanel.SetActive(false);

        helpPanel.SetActive(true);
        choiceCardUI.SetActive(true);
        lumaChatUI.SetActive(true);
    }

    public void OnLearnBookmarkClick()
    {
        puzzlePanel.SetActive(false);
        helpPanel.SetActive(false);
        learnPanel.SetActive(true);

        choiceCardUI.SetActive(false);
        lumaChatUI.SetActive(false);
    }
}
