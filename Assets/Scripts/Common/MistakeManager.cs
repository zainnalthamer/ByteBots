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

    [Header("Player Control")]
    [SerializeField] private PlayerControlToggle playerControlToggle;

    public bool blockGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    public void OnWrongAnswer(bool allowGameOver)
    {
        SaveManager.I.IncrementBugCountBy(-wrongAnswerPenalty);
        BugPointsManager.Instance.Refresh();

        if (allowGameOver && SaveManager.I.GetBugCount() <= 0)
        {
            TriggerGameOver();
        }
    }

    public void OnWrongAnswer()
    {
        OnWrongAnswer(true);
    }


    void TriggerGameOver()
    {
        if (blockGameOver)
            return;

        SoundController.Instance.PlaySFX(2);

        if (notebook && notebook.notebookRoot.activeSelf)
            notebook.CloseNotebook();

        if (playerControlToggle)
            playerControlToggle.DisableControl();

        if (gameOverCanvas) gameOverCanvas.SetActive(true);
        if (gameOverVolume) gameOverVolume.SetActive(true);
    }


    public void TryAgain()
    {
        Time.timeScale = 1f;

        if (playerControlToggle)
            playerControlToggle.EnableControl();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (playerControlToggle)
            playerControlToggle.EnableControl();

        SceneManager.LoadScene("MainMenu 1");
    }
}
