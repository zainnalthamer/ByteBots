using Fungus;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarouselPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkAnswerButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private BugGroup bugGroup;

    [Header("Carousel Control")]
    [SerializeField] private CarouselController carouselController;
    [SerializeField] private Transform horseRoot;

    private CarouselHorseController[] horses;

    [Header("Notebook Close")]
    [SerializeField] private GameObject notebookCanvasRoot;
    [SerializeField] private GameObject notebookBlurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Carousel NPC Reaction")]
    public Flowchart carouselFlowchart;
    public string solvedBlockName = "CarouselPuzzleSolvedReaction";

    [SerializeField] private Transform programmingEnv;

    private void Awake()
    {
        if (horseRoot)
        {
            horses = horseRoot.GetComponentsInChildren<CarouselHorseController>(true);
        }
    }


    public void ValidatePuzzle()
    {
        executionManager.Play();

        if (CheckBlocksForAnswer())
        {
            Debug.Log("[Carousel Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup) bugGroup.OnPuzzleSolved();

            ClearProgrammingEnv();

            if (carouselController) carouselController.ActivateCarousel();
            foreach (var h in horses) h.enabled = true;

            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            if (carouselFlowchart)
            {
                carouselFlowchart.ExecuteBlock(solvedBlockName);
            }

            QuestManager.Instance.OnPuzzleCompleted(11);

            //Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Carousel Puzzle] WRONG ANSWER!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool hasRotationsLabel = false;
        bool hasRotationsText = false;
        bool hasTen = false;
        bool hasOne = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            foreach (var input in inputs)
            {
                if (input == null) continue;

                string value = (input.StringValue ?? "").Trim().ToLower();
                if (string.IsNullOrEmpty(value)) continue;

                if (value == "rotations")
                    hasRotationsText = true;

                if (value == "10")
                    hasTen = true;

                if (value == "1")
                    hasOne = true;
            }

            var labels = block.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var label in labels)
            {
                if (label == null) continue;

                string txt = label.text.Trim().ToLower();
                if (txt == "rotations")
                    hasRotationsLabel = true;
            }
        }

        return hasRotationsLabel &&
               hasRotationsText &&
               hasTen &&
               hasOne;
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
