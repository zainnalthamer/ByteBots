using UnityEngine;

public class BookmarkController : MonoBehaviour
{
    [Header("Puzzle Panel")]
    public GameObject puzzlePanel;

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
}
