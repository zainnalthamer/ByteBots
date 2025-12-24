using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fungus;

public class CarnivalTicketValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Gate Collider")]
    [SerializeField] private Collider gateCollider;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Ticket Booth Reaction")]
    public Flowchart ticketFlowchart;
    public string solvedBlockName = "TicketPuzzleSolvedReaction";

    [SerializeField] private Transform programmingEnv;

    [Header("Help Concept")]
    [SerializeField] private string conceptName = "Functions";
    public string ConceptName => conceptName;

    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Ticket Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (gateCollider)
                gateCollider.enabled = false;

            if (bugGroup) bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            if (ticketFlowchart)
            {
                ticketFlowchart.ExecuteBlock(solvedBlockName);
            }

            QuestManager.Instance.OnPuzzleCompleted(10);

            //Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Ticket Puzzle] WRONG!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool foundHasTicketTrue = false;
        bool foundCanEnterTrue = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            if (inputs == null)
                continue;

            string varName = null;
            string varValue = null;
            int i = 0;

            foreach (var input in inputs)
            {
                if (i == 0)
                    varName = input.StringValue != null ? input.StringValue.Trim().ToLower() : "";
                else if (i == 1)
                {
                    varValue = input.StringValue != null ? input.StringValue.Trim().ToLower() : "";
                    break;
                }

                i++;
            }

            if (!string.IsNullOrEmpty(varName) && !string.IsNullOrEmpty(varValue))
            {
                if (varName == "hasticket" && varValue == "true")
                    foundHasTicketTrue = true;

                if (varName == "canenter" && varValue == "true")
                    foundCanEnterTrue = true;
            }
        }

        return foundHasTicketTrue && foundCanEnterTrue;
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