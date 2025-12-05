using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;

public class CropsGrowthValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button checkButton;
    [SerializeField] private Transform blocksContainer;
    [SerializeField] private CropsGrowthController cropsController;
    [SerializeField] private BugGroup bugGroup;

    [Header("UI")]
    [SerializeField] private GameObject puzzleUIRoot;
    [SerializeField] private GameObject blurVolume;
    [SerializeField] private MonoBehaviour playerFollowCamera;

    [Header("Expected Values")]
    [SerializeField] private float expectedNitrogen = 10f;
    [SerializeField] private float expectedPhosphorus = 15f;

    private const string nitrogenName = "nitrogen";
    private const string phosphorusName = "phosphorus";
    private const string totalName = "totalnutrients";

    private void Awake()
    {
        if (checkButton)
            checkButton.onClick.AddListener(ValidatePuzzle);
    }

    private void OnDestroy()
    {
        if (checkButton)
            checkButton.onClick.RemoveListener(ValidatePuzzle);
    }

    private void ValidatePuzzle()
    {
        StartCoroutine(ValidateRoutine());
    }

    private IEnumerator ValidateRoutine()
    {
        executionManager.Play();
        yield return new WaitForSeconds(0.1f);

        if (CheckSolution())
        {
            Debug.Log("[SoilMixPuzzle] CORRECT!");

            SoundController.Instance.PlaySFX(0);

            if (bugGroup)
                bugGroup.OnPuzzleSolved();

            if (cropsController)
                cropsController.GrowCrops();

            ClosePuzzleUI();
        }
        else
        {
            Debug.Log("[SoilMixPuzzle] Wrong answer — try again.");
            SoundController.Instance.PlaySFX(1);
            MistakeManager.Instance.OnWrongAnswer();
        }
    }

    private bool CheckSolution()
    {
        BE2_Block[] blocks = blocksContainer.GetComponentsInChildren<BE2_Block>(true);

        float foundNitrogen = Mathf.NegativeInfinity;
        float foundPhosphorus = Mathf.NegativeInfinity;
        float foundTotal = Mathf.NegativeInfinity;

        foreach (var block in blocks)
        {
            if (block == null) continue;

            string blockName = block.name.ToLower();
            if (!blockName.Contains("create") || !blockName.Contains("float"))
                continue;

            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);
            if (inputs.Length < 2)
                continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string varVal = inputs[1].StringValue.Trim();

            if (!float.TryParse(varVal, NumberStyles.Any, CultureInfo.InvariantCulture, out float value))
                continue;

            Debug.Log($"[Validator] Found {varName} = {value}");

            if (varName == nitrogenName)
                foundNitrogen = value;

            if (varName == phosphorusName)
                foundPhosphorus = value;

            if (varName == totalName)
                foundTotal = value;
        }

        float expectedTotal = expectedNitrogen + expectedPhosphorus;

        bool correct =
            Mathf.Approximately(foundNitrogen, expectedNitrogen) &&
            Mathf.Approximately(foundPhosphorus, expectedPhosphorus) &&
            Mathf.Approximately(foundTotal, expectedTotal);

        Debug.Log($"[Validator] N={foundNitrogen}, P={foundPhosphorus}, T={foundTotal}, expected T={expectedTotal}");

        return correct;
    }

    private void ClosePuzzleUI()
    {
        if (puzzleUIRoot)
            puzzleUIRoot.SetActive(false);

        if (blurVolume)
            blurVolume.SetActive(false);

        if (playerFollowCamera)
            playerFollowCamera.enabled = true;

        Time.timeScale = 1f;
    }
}
