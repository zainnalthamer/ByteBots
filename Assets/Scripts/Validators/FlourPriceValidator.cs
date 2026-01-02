using Fungus;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class FlourPriceValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Variable Names")]
    [SerializeField] private string pricePerCupName = "pricepercup";
    [SerializeField] private string cupsNeededName = "cupsneeded";
    [SerializeField] private string totalPriceName = "totalprice";

    [Header("Expected Values")]
    public float expectedPricePerCup = 1.5f;
    public float expectedCupsNeeded = 3f;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Mushy Reaction")]
    public Flowchart mushyFlowchart;
    public string mushySolvedBlock = "MushyPuzzleSolvedReaction";

    [SerializeField] private Transform programmingEnv;

    [Header("Help Concept")]
    [SerializeField] private string conceptName = "Operations";
    public string ConceptName => conceptName;

    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Flour Price Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot != null)
                notebookCanvasRoot.SetActive(false);

            if (notebookBlurVolume != null)
                notebookBlurVolume.SetActive(false);

            if (playerFollowCamera != null)
                playerFollowCamera.enabled = true;

            if (mushyFlowchart)
            {
                mushyFlowchart.ExecuteBlock(mushySolvedBlock);
            }

            QuestManager.Instance.OnPuzzleCompleted(6);

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Flour Price Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        float priceVal = -999f;
        float cupsVal = -999f;

        bool foundPrice = false;
        bool foundCups = false;
        bool foundTotal = false;

        foreach (var block in blocks)
        {
            string blockNameLower = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (inputs.Length < 2)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string varVal = inputs[1].StringValue.Trim();

            if (blockNameLower.Contains("create") && blockNameLower.Contains("float"))
            {
                if (varName == pricePerCupName)
                {
                    foundPrice = float.TryParse(varVal, out priceVal);
                }
                else if (varName == cupsNeededName)
                {
                    foundCups = float.TryParse(varVal, out cupsVal);
                }
                else if (varName == totalPriceName)
                {
                    foundTotal = true;
                }
            }
        }

        if (!foundPrice || !foundCups || !foundTotal)
            return false;

        if (Mathf.Abs(priceVal - expectedPricePerCup) > 0.001f)
            return false;

        if (Mathf.Abs(cupsVal - expectedCupsNeeded) > 0.001f)
            return false;

        return true;
    }

    void ClearProgrammingEnv()
    {
        if (!programmingEnv) return;

        for (int i = programmingEnv.childCount - 1; i >= 0; i--)
        {
            Destroy(programmingEnv.GetChild(i).gameObject);
        }
    }

}
