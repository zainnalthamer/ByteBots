using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Block;
using Fungus;

public class PrizePuzzleValidator : MonoBehaviour
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

    public Flowchart reactionFlowchart;
    public string solvedBlockName = "PrizePuzzleSolvedReaction";

    [SerializeField] private Transform programmingEnv;

    [Header("Help Concept")]
    [SerializeField] private string conceptName = "Functions";
    public string ConceptName => conceptName;

    [Header("End Game")]
    [SerializeField] private bool isFinalPuzzle = true;
    [SerializeField] private float endDelay = 3f;
    [SerializeField] private GameObject questManagerRoot;



    public void ValidatePuzzle()
    {
        MistakeManager.Instance.blockGameOver = true;

        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Prize Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup) bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            reactionFlowchart.StopAllCoroutines();
            reactionFlowchart.ExecuteBlock(solvedBlockName);

            if (isFinalPuzzle)
            {
                if (questManagerRoot)
                    questManagerRoot.SetActive(false);

                Invoke(nameof(EndGame), endDelay);
            }

            //Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Prize Puzzle] WRONG!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer(false);
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool hasCountTicketsLabel = false;
        bool hasTicketsText = false;
        bool hasNumberFive = false;
        bool hasWonPrizeText = false;
        bool hasTrueValue = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            foreach (var input in inputs)
            {
                if (input == null) continue;

                string value = (input.StringValue ?? "").Trim();

                if (string.IsNullOrEmpty(value))
                    continue;

                string lower = value.ToLower();

                if (lower == "tickets")
                    hasTicketsText = true;

                if (value == "5")
                    hasNumberFive = true;

                if (lower == "wonprize")
                    hasWonPrizeText = true;

                if (lower == "true")
                    hasTrueValue = true;
            }

            var labels = block.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var label in labels)
            {
                if (label == null) continue;

                string txt = label.text.Trim().ToLower();
                if (txt == "counttickets")
                    hasCountTicketsLabel = true;
            }
        }

        return hasCountTicketsLabel &&
               hasTicketsText &&
               hasNumberFive &&
               hasWonPrizeText &&
               hasTrueValue;
    }

    void ClearProgrammingEnv()
    {
        if (!programmingEnv) return;

        for (int i = programmingEnv.childCount - 1; i >= 0; i--)
        {
            Destroy(programmingEnv.GetChild(i).gameObject);
        }
    }

    private void EndGame()
    {
        Debug.Log("[GAME] Ending game");

        Application.Quit();
    }


}
