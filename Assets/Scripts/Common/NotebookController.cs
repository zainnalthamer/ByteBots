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

    [Header("Puzzle Validators")]
    public GameObject[] puzzleValidators;

    [Header("Extras")]
    public MonoBehaviour playerFollowCamera;
    public GameObject notebookBlurVolume;

    private void Start()
    {
        StartCoroutine(InitializeUI());
        DisableAllValidators();
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
            puzzleValidators[panelIndex].SetActive(true);
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

        DisableAllValidators();
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

    public void RunValidator()
    {
        //Check which validator is active and validate.
        foreach (var v in puzzleValidators)
        {
            if (v.activeSelf)
            {
                switch(v.name)
                {
                    case "GreenhouseSeeds Puzzle Validator":
                        var greenhouseValidator = v.GetComponent<GreenhouseSeedsValidator>();
                            greenhouseValidator.ValidatePuzzle(); 
                        break;
                    case "Water Well Puzzle Validator":
                        var wellValidator = v.GetComponent<WellFlowValidator>(); 
                            wellValidator.ValidatePuzzle();
                        break;
                    case "Cow Puzzle Validator":
                        var cowValidator = v.GetComponent<CowPuzzleValidator>();
                            cowValidator.ValidatePuzzle();
                        break;
                    case "Eggs Puzzle Validator":
                        var eggsValidator = v.GetComponent<EggsPuzzleValidator>();
                        eggsValidator.ValidatePuzzle();
                        break;
                    case "Flour Puzzle Validator":
                        var flourValidator = v.GetComponent<FlourPriceValidator>();
                        flourValidator.ValidatePuzzle();
                        break;
                    case "Food Puzzle Validator": 
                        var foodOrderValidator = v.GetComponent<FoodOrderPuzzleValidator>();
                            foodOrderValidator.ValidatePuzzle();
                        break;
                }
            }
        }
    }

    public void DisableAllValidators() { 
        foreach (var v in puzzleValidators)
            v.SetActive(false);
    }

}
