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

    [Header("Expected Values")]
    public int expectedPeachSeeds = 10;
    public int expectedPumpkinSeeds = 5;
    public float expectedTotalCost = 15f;

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

        int peachValue = -999;
        int pumpkinValue = -999;
        float totalValue = -999f;

        bool foundPeach = false;
        bool foundPumpkin = false;
        bool foundTotal = false;

        foreach (var block in blocks)
        {
            string blockName = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (inputs.Length < 2)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string varVal = inputs[1].StringValue.Trim();

            if (blockName.Contains("create") && blockName.Contains("int"))
            {
                if (varName == peachSeedsName.ToLower())
                {
                    foundPeach = int.TryParse(varVal, out peachValue);
                    Debug.Log($"[Validator] peachSeeds = {peachValue}");
                }
                else if (varName == pumpkinSeedsName.ToLower())
                {
                    foundPumpkin = int.TryParse(varVal, out pumpkinValue);
                    Debug.Log($"[Validator] pumpkinSeeds = {pumpkinValue}");
                }
            }

            if (blockName.Contains("create") && blockName.Contains("float"))
            {
                if (varName == totalCostName.ToLower())
                {
                    foundTotal = true;
                    Debug.Log("[Validator] totalCost variable found (value ignored)");
                }
            }
        }

        if (!foundPeach || !foundPumpkin || !foundTotal)
            return false;

        if (peachValue != expectedPeachSeeds)
            return false;

        if (pumpkinValue != expectedPumpkinSeeds)
            return false;

        return true;
    }
}
