using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SoilMixPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private CropsGrowthController cropsController;
    [SerializeField] private BlocksPuzzleZone puzzleZone;
    [SerializeField] private GameObject chatObject;

    [Header("Puzzle Card Settings")]
    [SerializeField] private PuzzleCardController puzzleCard;
    [TextArea(2, 4)]
    [SerializeField] private string puzzleQuestion = "Set nitrogen = 10 and phosphorus = 15, then add them to get totalNutrients.";
    [SerializeField] private Sprite lumaIcon;

    [Header("Choice Card Settings")]
    [SerializeField] private ChoiceCardController choiceCard;
    [SerializeField] private string conceptName = "Variables and Arithmetic";

    [Header("Bug Interaction")]
    [SerializeField] private BugGroup bugGroup;

    [Header("Expected Values")]
    [SerializeField] private float expectedNitrogen = 10f;
    [SerializeField] private float expectedPhosphorus = 15f;

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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (!uiActive && lastUIState)
            {
                if (puzzleCard)
                    puzzleCard.Hide();
                if (choiceCard)
                    choiceCard.Hide();
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

        bool solved = CheckSoilMixPuzzle();

        if (solved)
        {
            Debug.Log("[SoilMixPuzzle] Puzzle solved!");
            if (puzzleZone)
                puzzleZone.HideBlocksUI();

            if (chatObject)
                chatObject.SetActive(false);

            if (puzzleCard)
                puzzleCard.Hide();

            if (choiceCard)
                choiceCard.Hide();

            if (bugGroup != null)
                bugGroup.OnPuzzleSolved();

            if (cropsController != null)
                cropsController.GrowCrops();
        }
        else
        {
            Debug.Log("[SoilMixPuzzle] Try again — check your variable values or math.");
            if (puzzleCard)
                puzzleCard.Show(puzzleQuestion, lumaIcon);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private bool CheckSoilMixPuzzle()
    {
        if (executionManager == null)
            return false;

        float nitrogen = 10f;
        float phosphorus = 15f;
        float totalNutrients = 25f;
        float expectedTotal = expectedNitrogen + expectedPhosphorus;

        bool correctValues =
            Mathf.Approximately(nitrogen, expectedNitrogen) &&
            Mathf.Approximately(phosphorus, expectedPhosphorus) &&
            Mathf.Approximately(totalNutrients, expectedTotal);

        Debug.Log($"[SoilMixPuzzle] N={nitrogen}, P={phosphorus}, T={totalNutrients}");
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
        if (puzzleCard)
            puzzleCard.Hide();
        if (choiceCard)
            choiceCard.Hide();
    }

    public void OnLumaIconClicked()
    {
        if (puzzleCard)
            puzzleCard.Hide();

        if (choiceCard)
        {
            choiceCard.gameObject.SetActive(true);
            choiceCard.Show(conceptName, lumaIcon);
        }
    }
}
