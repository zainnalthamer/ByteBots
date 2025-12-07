using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class GreenhouseSeedsValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Variable Names")]
    [SerializeField] private string peachSeedsName = "peachSeeds";
    [SerializeField] private string pumpkinSeedsName = "pumpkinSeeds";
    [SerializeField] private string totalCostName = "totalCost";

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    private void Awake()
    {
        checkAnswerButton.onClick.AddListener(ValidatePuzzle);
    }

    private void OnDestroy()
    {
        checkAnswerButton.onClick.RemoveListener(ValidatePuzzle);
    }

    private void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            SoundController.Instance.PlaySFX(0);
            Debug.Log("[Greenhouse Puzzle] CORRECT!");

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            if (notebookCanvasRoot != null)
                notebookCanvasRoot.SetActive(false);

            if (notebookBlurVolume != null)
                notebookBlurVolume.SetActive(false);

            if (playerFollowCamera != null)
                playerFollowCamera.enabled = true;

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Greenhouse Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool hasPeach = false;
        bool hasPumpkin = false;
        bool hasTotalCost = false;

        string peachNameLower = peachSeedsName.ToLower();
        string pumpkinNameLower = pumpkinSeedsName.ToLower();
        string totalCostNameLower = totalCostName.ToLower();

        foreach (var block in blocks)
        {
            string blockName = block.name.ToLower();

            if (blockName.Contains("create") && blockName.Contains("int"))
            {
                var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
                if (inputs.Length < 2)
                    continue;

                string varName = inputs[0].StringValue.Trim().ToLower();
                string varVal = inputs[1].StringValue.Trim();

                Debug.Log("[Greenhouse Validator] INT var " + varName + " = " + varVal);

                if (varName == peachNameLower)
                    hasPeach = true;
                else if (varName == pumpkinNameLower)
                    hasPumpkin = true;
            }
            
            else if (blockName.Contains("create") && blockName.Contains("float"))
            {
                var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
                if (inputs.Length < 2)
                    continue;

                string varName = inputs[0].StringValue.Trim().ToLower();
                string varVal = inputs[1].StringValue.Trim();

                Debug.Log("[Greenhouse Validator] FLOAT var " + varName + " = " + varVal);

                if (varName == totalCostNameLower)
                    hasTotalCost = true;
            }
        }

        return hasPeach && hasPumpkin && hasTotalCost;
    }
}
