using Fungus;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CropsPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private CropsGrowthController cropsController;
    [SerializeField] private BlocksPuzzleZone puzzleZone;

    [SerializeField] private NotebookController notebookController;
    [SerializeField] private int notebookPanelIndex = 0;

    //[Header("Bug Interaction")]
    //[SerializeField] private BugGroup bugGroup;

    [Header("Expected Values")]
    [SerializeField] private string variableName = "phosphorus";
    [SerializeField] private float expectedNitrogen = 10f;
    [SerializeField] private float expectedPhosphorus = 15f;
    public List<string> expectedBlockNames = new List<string>();

    private bool lastUIState;

    private void Awake()
    {
        if (playButton)
            playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDestroy()
    {
        if (playButton)
            playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private bool hasEnteredTrigger = false;

    private void Update()
    {
        if (!hasEnteredTrigger) return;

        bool uiActive = puzzleZone.gameObject.activeInHierarchy;

        if (uiActive && !lastUIState)
        {
            StartCoroutine(ShowPuzzleCard());
            ControlsManager.Instance.ShowCursor();
        }

        lastUIState = uiActive;
    }


    private IEnumerator ShowPuzzleCard()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();

        if (notebookController != null)
            notebookController.OpenNotebook(notebookPanelIndex);

        yield return null;
        Canvas.ForceUpdateCanvases();
    }

    private void OnPlayClicked()
    {
        StartCoroutine(ValidateAfterDelay());
    }

    private IEnumerator ValidateAfterDelay()
    {
        yield return null;

        executionManager.Play();
        yield return new WaitForSeconds(0.1f);

        bool solved = CheckSoilMixPuzzle();

        if (solved)
        {
            Debug.Log("[SoilMixPuzzle] Puzzle solved!");
            if (puzzleZone)
                puzzleZone.HideBlocksUI();

            //if (bugGroup != null)
            //    bugGroup.OnPuzzleSolved();

            if (cropsController != null)
                cropsController.GrowCrops();
        }
        else
        {
            Debug.Log("[SoilMixPuzzle] Try again — check your variable values or math.");

            ControlsManager.Instance.ShowCursor();
        }
    }

    private bool CheckSoilMixPuzzle()
    { 
        if (executionManager == null)
            return false;

        Debug.LogWarning("[Validator] Checking blocks for correct variable assignment...");

        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));

        //foreach (var block in allBlocks)
        for(int i = 0; i < allBlocks.Count; i++)
        {
            var block = allBlocks[i]; 

            Debug.Log(block.name);

            // if (block == null) continue;
             
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            //if (inputs.Length < 2) continue;
             
            if (block.name.Contains("Block Function Instance"))
            {
                string varName = inputs[0].StringValue.Trim().ToLower();
                string varValue = inputs[1].StringValue.Trim();

                if (varName == variableName.ToLower() && ValuesEqual(varValue, expectedPhosphorus.ToString()))
                {
                    Debug.Log($"[Validator] Found correct block: {varName} = {varValue}");
                    return true;
                }
            }
        }
        Debug.Log("[Validator] No matching block found.");
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        hasEnteredTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
    }

    private bool ValuesEqual(string a, string b)
    {
        if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var da) &&
            double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var db))
            return Mathf.Abs((float)(da - db)) < 0.0001f;

        return string.Equals(a.Trim(), b.Trim(), System.StringComparison.Ordinal);
    }
}
