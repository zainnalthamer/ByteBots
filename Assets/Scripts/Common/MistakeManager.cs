using UnityEngine;
using UnityEngine.SceneManagement;

public class MistakeManager : MonoBehaviour
{
    public static MistakeManager Instance;

    [Header("Game Over UI")]
    public GameObject gameOverCanvas;
    public GameObject gameOverVolume;
    public NotebookController notebook;

    [Header("Settings")]
    public int wrongAnswerPenalty = 10;

    void Awake()
    {
        Instance = this;
    }

    public void OnWrongAnswer()
    {
        SaveManager.I.IncrementBugCountBy(-wrongAnswerPenalty);

        BugPointsManager.Instance.Refresh();

        if (SaveManager.I.GetBugCount() <= 0)
        {
            TriggerGameOver();
        }
    }

    void TriggerGameOver()
    {
        SoundController.Instance.PlaySFX(2);

        if (notebook && notebook.notebookRoot.activeSelf)
            notebook.CloseNotebook();

       // Time.timeScale = 0f;

        if (gameOverCanvas) gameOverCanvas.SetActive(true);
        if (gameOverVolume) gameOverVolume.SetActive(true);
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu 1");
    }
}
