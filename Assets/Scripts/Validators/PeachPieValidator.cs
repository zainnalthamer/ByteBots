using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class PeachPieValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Pie Object")]
    [SerializeField] private GameObject pieObject;

    [Header("Expected Variable Names")]
    public string peachVar = "peachCount";
    public string milkVar = "milkStatus";
    public string eggsVar = "totalEggs";
    public string flourVar = "flourCups";

    [Header("Expected Values")]
    public int expectedPeaches = 4;
    public string expectedMilk = "clean";
    public int expectedEggs = 5;
    public float expectedFlour = 3.0f;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;


    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Peach Pie Puzzle] CORRECT!");

            SoundController.Instance.PlaySFX(0);

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            if (pieObject != null)
                pieObject.SetActive(true);

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Peach Pie Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool foundPeach = false;
        bool foundMilk = false;
        bool foundEggs = false;
        bool foundFlour = false;

        int peachValue = -999;
        int eggsValue = -999;
        float flourValue = -999f;
        string milkValue = "";

        foreach (var block in blocks)
        {
            string blockName = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (inputs.Length < 2)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string varVal = inputs[1].StringValue.Trim().ToLower();

            if (blockName.Contains("create") && blockName.Contains("int"))
            {
                if (varName == peachVar.ToLower())
                {
                    foundPeach = int.TryParse(varVal, out peachValue);
                }
                else if (varName == eggsVar.ToLower())
                {
                    foundEggs = int.TryParse(varVal, out eggsValue);
                }
            }

            if (blockName.Contains("create") && blockName.Contains("float"))
            {
                if (varName == flourVar.ToLower())
                {
                    foundFlour = float.TryParse(varVal, out flourValue);
                }
            }

            if (blockName.Contains("create") && blockName.Contains("string"))
            {
                if (varName == milkVar.ToLower())
                {
                    foundMilk = true;
                    milkValue = varVal;
                }
            }
        }

        if (!foundPeach || !foundMilk || !foundEggs || !foundFlour)
            return false;

        if (peachValue != expectedPeaches) return false;
        if (eggsValue != expectedEggs) return false;
        if (milkValue != expectedMilk.ToLower()) return false;

        if (Mathf.Abs(flourValue - expectedFlour) > 0.01f) return false;

        return true;
    }
}
