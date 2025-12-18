using Fungus;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class SeafoodFreshnessValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Coral Finster Reaction")]
    public Flowchart coralFlowchart;
    public string solvedBlockName = "SeafoodPuzzleSolvedReaction";


    public void ValidatePuzzle()
    {
        Time.timeScale = 1f;
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Seafood Freshness Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;
             
                coralFlowchart.ExecuteBlock(solvedBlockName); 

            
        }
        else
        {
            Debug.Log("[Seafood Freshness Puzzle] WRONG!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        BE2_Block firstString = null;
        BE2_Block secondString = null;
        BE2_Block ifBlock = null;
        BE2_Block boolBlockInsideIf = null;

        foreach (var block in blocks)
        {
            string name = block.name.ToLower();

            if (name.Contains("create") && name.Contains("string"))
            {
                if (firstString == null) firstString = block;
                else if (secondString == null) secondString = block;
            }
            else if (name.Contains("if"))
            {
                ifBlock = block;
            }
            else if (name.Contains("create") && name.Contains("bool"))
            {
                boolBlockInsideIf = block;
            }
        }

        if (firstString == null || secondString == null || ifBlock == null || boolBlockInsideIf == null)
            return false;

        if (!BlockMatchesString(firstString, "tempstatus", "cold"))
            return false;

        if (!BlockMatchesString(secondString, "texturestatus", "firm"))
            return false;

        if (!BlockMatchesBool(boolBlockInsideIf, "isfresh", "true"))
            return false;

        if (!boolBlockInsideIf.transform.IsChildOf(ifBlock.transform))
            return false;

        return true;
    }

    private bool BlockMatchesString(BE2_Block block, string expectedName, string expectedValue)
    {
        var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
        if (inputs.Length < 2) return false;

        string varName = inputs[0].StringValue.Trim().ToLower();
        string varValue = inputs[1].StringValue.Trim().ToLower().Replace("\"", "");

        return varName == expectedName && varValue == expectedValue;
    }

    private bool BlockMatchesBool(BE2_Block block, string expectedName, string expectedValue)
    {
        var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
        if (inputs.Length < 2) return false;

        string varName = inputs[0].StringValue.Trim().ToLower();
        string varValue = inputs[1].StringValue.Trim().ToLower().Replace("\"", "");

        return varName == expectedName && varValue == expectedValue;
    }

}
