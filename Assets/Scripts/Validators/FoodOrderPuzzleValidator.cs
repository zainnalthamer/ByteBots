using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Block;

public class FoodOrderPuzzleValidator : MonoBehaviour
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
            Debug.Log("[Food Order Puzzle] CORRECT!");
            SoundController.Instance.PlaySFX(0);

            if (bugGroup) bugGroup.OnPuzzleSolved();
            if (notebookCanvasRoot) notebookCanvasRoot.SetActive(false);
            if (notebookBlurVolume) notebookBlurVolume.SetActive(false);
            if (playerFollowCamera) playerFollowCamera.enabled = true;

            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("[Food Order Puzzle] WRONG!");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckBlocksForAnswer()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        bool hasPrepareOrderLabel = false;
        bool hasOrderStringTaco = false;
        bool hasPrepareOrderCall = false;
        bool hasIfOrderEqualsTaco = false;
        bool hasIsReadyTrue = false;

        foreach (var block in blocks)
        {
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            foreach (var input in inputs)
            {
                if (input == null)
                    continue;

                string value = (input.StringValue ?? "").Trim().ToLower();
                if (string.IsNullOrEmpty(value))
                    continue;

                if (value == "order")
                    hasOrderStringTaco = true;

                if (value == "taco")
                    hasOrderStringTaco = true;

                if (value.Contains("order") || value.Contains("taco"))
                    hasIfOrderEqualsTaco = true;

                if (value == "isready")
                    hasIsReadyTrue = true;

                if (value == "true")
                    hasIsReadyTrue = true;
            }

            var labels = block.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var label in labels)
            {
                if (label == null)
                    continue;

                string txt = label.text.Trim().ToLower();

                if (txt == "prepareorder")
                    hasPrepareOrderLabel = true;

                if (txt == "prepareorder")
                    hasPrepareOrderCall = true;
            }
        }

        return
            hasPrepareOrderLabel &&
            hasOrderStringTaco &&
            hasPrepareOrderCall &&
            hasIfOrderEqualsTaco &&
            hasIsReadyTrue;
    }
}
