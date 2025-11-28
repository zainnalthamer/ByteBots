using System.Collections;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    [Header("Notebook Root")]
    public GameObject notebookRoot;

    [Header("Puzzle Panels")]
    public GameObject[] puzzlePanels;

    private void Start()
    {
        StartCoroutine(InitializeUI());
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        notebookRoot.SetActive(false);
        HideAllPanels();
    }

    public void OpenNotebook(int panelIndex)
    {
        notebookRoot.SetActive(true);
        HideAllPanels();

        if (panelIndex >= 0 && panelIndex < puzzlePanels.Length)
            puzzlePanels[panelIndex].SetActive(true);

        ControlsManager.Instance.ShowCursor();
        Time.timeScale = 0f;
    }

    public void CloseNotebook()
    {
        notebookRoot.SetActive(false);
        Time.timeScale = 1f;
        ControlsManager.Instance.HideCursor();

        HideAllPanels();
    }

    private void HideAllPanels()
    {
        foreach (var p in puzzlePanels)
            p.SetActive(false);
    }
}
