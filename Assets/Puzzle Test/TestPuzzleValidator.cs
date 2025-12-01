using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class TestPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Visual Feedback")]
    [SerializeField] private Renderer cubeRenderer;
    [SerializeField] private Material correctMaterial;

    [Header("Expected Answer")]
    [SerializeField] private string expectedVariableName = "employees";
    [SerializeField] private string expectedValue = "20";

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
            Debug.Log("[Puzzle] CORRECT!");

            // turn cube green
            cubeRenderer.material = correctMaterial;

            // bug animation
            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();
        }
        else
        {
            Debug.Log("[Puzzle] WRONG ANSWER");
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

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

                Debug.Log($"[Validator] Found {varName} = {varVal}");

                if (varName == expectedVariableName.ToLower() &&
                    varVal == expectedValue)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
