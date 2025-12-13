using System.Collections;
using UnityEngine;

public class NotebookController : MonoBehaviour
{
    [Header("Notebook Root")]
    public GameObject notebookRoot;

    [Header("Puzzle Panels")]
    public GameObject[] puzzlePanels;

    [Header("Puzzle Block Sets")]
    public GameObject[] puzzleBlockSets;

    [Header("Extras")]  
    public MonoBehaviour playerFollowCamera; 
    public GameObject notebookBlurVolume; 

    private void Start()
    {
        StartCoroutine(InitializeUI());
    }

    private IEnumerator InitializeUI()
    {
        yield return null;

        notebookRoot.SetActive(false);
        HideAllPanels();
        HideAllBlockSets();

        if (notebookBlurVolume)
            notebookBlurVolume.SetActive(false);
    }


    public void OpenNotebook(int panelIndex)
    {
        notebookRoot.SetActive(true);
        HideAllPanels();
        HideAllBlockSets();

        if (panelIndex >= 0 && panelIndex < puzzlePanels.Length)
        {
            puzzlePanels[panelIndex].SetActive(true);
            puzzleBlockSets[panelIndex].SetActive(true);
        }

        Time.timeScale = 0f;

        if (playerFollowCamera)
            playerFollowCamera.enabled = false;

        if (notebookBlurVolume)
            notebookBlurVolume.SetActive(true);
    }


    public void CloseNotebook()
    {
        notebookRoot.SetActive(false);
        Time.timeScale = 1f;

        HideAllPanels();
        HideAllBlockSets();

        if (playerFollowCamera)
            playerFollowCamera.enabled = true;

        if (notebookBlurVolume)
            notebookBlurVolume.SetActive(false);
    }

    public void QuitNotebook()
    {
        notebookRoot.SetActive(false);
        Time.timeScale = 1f;

        HideAllPanels();

        if (playerFollowCamera)
            playerFollowCamera.enabled = true;

        if (notebookBlurVolume)
            notebookBlurVolume.SetActive(false);
    }

    private void HideAllPanels()
    {
        foreach (var p in puzzlePanels)
            p.SetActive(false);
    }

    private void HideAllBlockSets()
    {
        foreach (var b in puzzleBlockSets)
            b.SetActive(false);
    }

}
