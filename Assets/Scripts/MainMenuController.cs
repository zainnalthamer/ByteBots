using UnityEngine; 

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "Carnival";

    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject mainButtons;

    void Start()
    {
        ShowMain();
    }

    public void PlayGame()
    {
        //SceneManager.LoadScene(gameSceneName);
        SceneLoader.Instance.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        mainButtons.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        mainButtons.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void BackFromOptions()
    {
        ShowMain();
    }

    public void BackFromCredits()
    {
        ShowMain();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ShowMain()
    {
        mainButtons.SetActive(true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
}
