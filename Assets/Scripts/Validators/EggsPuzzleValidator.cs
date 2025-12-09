using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class EggsPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Variable Names")]
    [SerializeField] private string brownEggsName = "brownEggs";
    [SerializeField] private string whiteEggsName = "whiteEggs";
    [SerializeField] private string totalEggsName = "totalEggs";

    [Header("Expected Values")]
    public int expectedBrown = 2;
    public int expectedWhite = 3;

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
            Debug.Log("[Eggs Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

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
            Debug.Log("[Eggs Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        int brownVal = -999;
        int whiteVal = -999;

        bool foundBrown = false;
        bool foundWhite = false;
        bool foundTotal = false;

        foreach (var block in blocks)
        {
            string blockNameLower = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (inputs.Length < 2)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string varVal = inputs[1].StringValue.Trim();

            if (blockNameLower.Contains("create") && blockNameLower.Contains("int"))
            {
                if (varName == brownEggsName.ToLower())
                {
                    foundBrown = int.TryParse(varVal, out brownVal);
                }
                else if (varName == whiteEggsName.ToLower())
                {
                    foundWhite = int.TryParse(varVal, out whiteVal);
                }
                else if (varName == totalEggsName.ToLower())
                {
                    foundTotal = true;
                }
            }
        }

        if (!foundBrown || !foundWhite || !foundTotal)
            return false;

        if (brownVal != expectedBrown)
            return false;

        if (whiteVal != expectedWhite)
            return false;

        return true;
    }
}
