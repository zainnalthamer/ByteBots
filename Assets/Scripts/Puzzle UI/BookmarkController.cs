using UnityEngine;

public class BookmarkController : MonoBehaviour
{
    public GameObject puzzlePanel;
    public GameObject helpPanel;

    public void OnPuzzleBookmarkClick()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
            Debug.Log("Switched to Puzzle View");
        }
        else
        {
            Debug.LogWarning("Puzzle Panel is not assigned!");
        }
    }

    public void OnHelpBookmarkClick()
    {
        if (helpPanel != null)
        {
            puzzlePanel.SetActive(false);
            helpPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Help Panel is not assigned!");
        }
    }
}
