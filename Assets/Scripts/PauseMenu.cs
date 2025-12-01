using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    public static bool GameIsPaused = false;
    public GameObject pauseBlurVolume;
    public GameObject playerFollowCamera;
    public NotebookController notebookController;
    public GameObject notebookCanvasRoot;
    public GameObject openNotebookPrompt;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseBlurVolume.SetActive(false);
        playerFollowCamera.SetActive(true);

        if (notebookCanvasRoot.activeSelf)
        {
            notebookController.CloseNotebook();
        }

        if (openNotebookPrompt != null)
            openNotebookPrompt.SetActive(true);

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        if (notebookCanvasRoot.activeSelf)
        {
            notebookController.CloseNotebook();
        }

        pauseMenuUI.SetActive(true);
        pauseBlurVolume.SetActive(true);
        playerFollowCamera.SetActive(false);

        if (openNotebookPrompt != null)
            openNotebookPrompt.SetActive(false);

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
