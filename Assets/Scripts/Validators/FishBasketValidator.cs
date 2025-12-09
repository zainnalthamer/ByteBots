using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class FishBasketValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Variable Names")]
    public string basketVar = "basketCapacity";
    public string fishVar = "fishCaught";
    public string overflowVar = "isOverflowing";

    [Header("Expected Values")]
    public int expectedBasket = 8;
    public int expectedFish = 6;

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
            Debug.Log("[Fish Basket Puzzle] CORRECT!");

            SoundController.Instance.PlaySFX(0);

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Fish Basket Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool foundBasket = false;
        bool foundFish = false;
        bool foundOverflow = false;

        foreach (var block in blocks)
        {
            string blockName = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (inputs.Length < 1)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();

            if (blockName.Contains("create") && blockName.Contains("int"))
            {
                if (varName == basketVar.ToLower())
                    foundBasket = true;
                else if (varName == fishVar.ToLower())
                    foundFish = true;
            }

            if (blockName.Contains("create") && blockName.Contains("bool"))
            {
                if (varName == overflowVar.ToLower())
                    foundOverflow = true;
            }
        }

        return foundBasket && foundFish && foundOverflow;
    }
}
