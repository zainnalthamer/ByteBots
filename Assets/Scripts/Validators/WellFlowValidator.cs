using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class WellFlowValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;
    [SerializeField] private CropsGrowthController cropsController;

    [Header("Expected Variable Names")]
    [SerializeField] private string currentWaterName = "currentWater";
    [SerializeField] private string requiredWaterName = "requiredWater";
    [SerializeField] private string boolName = "isEnough";

    [Header("Expected Values")]
    public float expectedCurrentWater = 7.8f;
    public float expectedRequiredWater = 6.5f;
    public bool expectedIsEnough = true;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [SerializeField] private Transform programmingEnv;

    [Header("Help Concept")]
    [SerializeField] private string conceptName = "Data Types";
    public string ConceptName => conceptName;

    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            SoundController.Instance.PlaySFX(0);
            Debug.Log("[Well Puzzle] CORRECT!");

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot != null)
                notebookCanvasRoot.SetActive(false);

            if (notebookBlurVolume != null)
                notebookBlurVolume.SetActive(false);

            if (cropsController != null)
                cropsController.GrowCrops();

            if (playerFollowCamera != null)
                playerFollowCamera.enabled = true;

            QuestManager.Instance.OnPuzzleCompleted(3);

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Well Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool foundCurrentWater = false;
        bool foundRequiredWater = false;
        bool foundBool = false;

        float currentWaterValue = -999f;
        float requiredWaterValue = -999f;
        bool boolValue = false;

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
                if (varName == currentWaterName.ToLower())
                {
                    foundCurrentWater = float.TryParse(varVal, out currentWaterValue);
                    Debug.Log($"[Validator] currentWater = {currentWaterValue}");
                }
                else if (varName == requiredWaterName.ToLower())
                {
                    foundRequiredWater = float.TryParse(varVal, out requiredWaterValue);
                    Debug.Log($"[Validator] requiredWater = {requiredWaterValue}");
                }
            }

            if (blockNameLower.Contains("create") && blockNameLower.Contains("bool"))
            {
                if (varName == boolName.ToLower())
                {
                    foundBool = bool.TryParse(varVal.ToLower(), out boolValue);
                    Debug.Log($"[Validator] isEnough = {boolValue}");
                }
            }
        }

        if (!foundCurrentWater || !foundRequiredWater || !foundBool)
        {
            Debug.Log("[Validator] Missing required variables.");
            return false;
        }

        if (Mathf.Abs(currentWaterValue - expectedCurrentWater) > 0.001f)
            return false;

        if (Mathf.Abs(requiredWaterValue - expectedRequiredWater) > 0.001f)
            return false;

        if (boolValue != expectedIsEnough)
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
