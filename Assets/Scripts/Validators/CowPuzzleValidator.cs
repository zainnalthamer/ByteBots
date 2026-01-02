using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using UnityEngine;
using UnityEngine.UI;

public class CowPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Variable Names")]
    [SerializeField] private string hungerVarName = "hungerStatus";
    [SerializeField] private string moodVarName = "moodStatus";
    [SerializeField] private string stallVarName = "stallStatus";

    [Header("Expected Values")]
    public string expectedHunger = "hungry";
    public string expectedMood = "grumpy";
    public string expectedStall = "dirty";

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Fungus Cutscene")]
    [SerializeField] private Fungus.Flowchart flowchart;
    [SerializeField] private string blockNameToPlay = "MilkCow";

    [SerializeField] private Transform programmingEnv;

    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Cow Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (notebookCanvasRoot != null)
                notebookCanvasRoot.SetActive(false);

            if (notebookBlurVolume != null)
                notebookBlurVolume.SetActive(false);

            if (playerFollowCamera != null)
                playerFollowCamera.enabled = true;

            if (flowchart != null)
            {
                flowchart.ExecuteBlock(blockNameToPlay);
            }

            QuestManager.Instance.OnPuzzleCompleted(4);

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Cow Puzzle] WRONG ANSWER");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool foundHunger = false;
        bool foundMood = false;
        bool foundStall = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            if (inputs.Length == 0) continue;

            string varName = inputs[0].StringValue.Trim().ToLower();

            if (varName == hungerVarName.ToLower())
                foundHunger = true;

            if (varName == moodVarName.ToLower())
                foundMood = true;

            if (varName == stallVarName.ToLower())
                foundStall = true;
        }

        return foundHunger && foundMood && foundStall;
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
