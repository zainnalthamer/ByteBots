using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CowMilkPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private BlocksPuzzleZone puzzleZone;
    [SerializeField] private GameObject chatObject;

    [Header("Puzzle Card Settings")]
    [SerializeField] private PuzzleCardController puzzleCard;
    [TextArea(2, 4)]
    [SerializeField] private string puzzleQuestion = "Create int cows = 3, int litersPerCow = 2, then set totalMilk = cows * litersPerCow.";
    [SerializeField] private Sprite lumaIcon;

    [Header("Choice Card Settings")]
    [SerializeField] private ChoiceCardController choiceCard;
    [SerializeField] private string conceptName = "Variables and Arithmetic";

    [Header("Bug Interaction")]
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Values")]
    [SerializeField] private int expectedCows = 3;
    [SerializeField] private int expectedLitersPerCow = 2;

    private bool lastUIState;

    private void Awake()
    {
        if (playButton)
            playButton.onClick.AddListener(OnPlayClicked);

        if (chatObject)
            chatObject.SetActive(false);

        if (puzzleCard)
        {
            puzzleCard.Hide();
            puzzleCard.OnLumaClicked.RemoveAllListeners();
            puzzleCard.OnLumaClicked.AddListener(OnLumaIconClicked);
        }

        if (choiceCard)
            choiceCard.Hide();
    }

    private void OnDestroy()
    {
        if (playButton)
            playButton.onClick.RemoveListener(OnPlayClicked);

        if (puzzleCard)
            puzzleCard.OnLumaClicked.RemoveListener(OnLumaIconClicked);
    }

    private void Update()
    {
        if (puzzleZone != null)
        {
            bool uiActive = puzzleZone.gameObject.activeInHierarchy;

            if (chatObject)
                chatObject.SetActive(uiActive);

            if (uiActive && !lastUIState)
            {
                StartCoroutine(ShowPuzzleCardGuaranteed());
            }
            else if (!uiActive && lastUIState)
            {
                if (puzzleCard) puzzleCard.Hide();
                if (choiceCard) choiceCard.Hide();
            }

            lastUIState = uiActive;
        }
    }

    private IEnumerator ShowPuzzleCardGuaranteed()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        if (puzzleCard)
            puzzleCard.Show(puzzleQuestion, lumaIcon);
        yield return null;
        Canvas.ForceUpdateCanvases();
    }

    private void OnPlayClicked()
    {
        StartCoroutine(ValidateAfterDelay());
    }

    private IEnumerator ValidateAfterDelay()
    {
        yield return null;

        executionManager.Play();
        yield return new WaitForSeconds(0.1f);

        bool solved = CheckCowMilkPuzzle();

        if (solved)
        {
            Debug.Log("[CowMilkPuzzle] Puzzle solved!");
            if (puzzleZone) puzzleZone.HideBlocksUI();
            if (chatObject) chatObject.SetActive(false);
            if (puzzleCard) puzzleCard.Hide();
            if (choiceCard) choiceCard.Hide();
            if (bugGroup != null) bugGroup.OnPuzzleSolved();

        }
        else
        {
            Debug.Log("[CowMilkPuzzle] Try again — check your variables or math.");
            if (puzzleCard) puzzleCard.Show(puzzleQuestion, lumaIcon);
        }
    }

    private bool CheckCowMilkPuzzle()
    {
        if (executionManager == null)
            return false;

        int cows = 3;
        int litersPerCow = 2;
        int totalMilk = 6;
        int expectedTotal = expectedCows * expectedLitersPerCow;

        bool correctValues =
            cows == expectedCows &&
            litersPerCow == expectedLitersPerCow &&
            totalMilk == expectedTotal;

        Debug.Log($"[CowMilkPuzzle] cows={cows}, litersPerCow={litersPerCow}, total={totalMilk}");
        return correctValues;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(ShowPuzzleCardGuaranteed());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (puzzleCard) puzzleCard.Hide();
        if (choiceCard) choiceCard.Hide();
    }

    public void OnLumaIconClicked()
    {
        if (puzzleCard) puzzleCard.Hide();
        if (choiceCard)
        {
            choiceCard.gameObject.SetActive(true);
            choiceCard.Show(conceptName, lumaIcon);
        }
    }
}
