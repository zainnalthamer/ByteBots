using Fungus;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class FishPriceValidator : MonoBehaviour
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

    [Header("Fish Price NPC Reaction")]
    public Flowchart fishPriceFlowchart;
    public string solvedBlockName = "FishPricePuzzleSolvedReaction";

    [SerializeField] private Transform programmingEnv;

    [Header("Help Concept")]
    [SerializeField] private string conceptName = "Conditionals";
    public string ConceptName => conceptName;

    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Fish Price Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup) bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            if (fishPriceFlowchart)
            {
                fishPriceFlowchart.ExecuteBlock(solvedBlockName);
            }

            QuestManager.Instance.OnPuzzleCompleted(8);

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Fish Price Puzzle] WRONG!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool hasFishType = false;
        bool hasPrice = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            if (inputs.Length < 2) continue;

            string varName = inputs[0].StringValue.Trim().ToLower();

            if (varName == "fishtype")
                hasFishType = true;

            if (varName == "price")
                hasPrice = true;
        }

        return hasFishType && hasPrice;
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
