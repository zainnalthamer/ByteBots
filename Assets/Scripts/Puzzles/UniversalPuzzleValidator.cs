using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class UniversalPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private BlocksPuzzleZone puzzleZone;
    [SerializeField] private GameObject chatObject;
    [SerializeField] private GameObject lumaChatUI;
    [SerializeField] private DoorController doorController;

    [Header("Expected Values")]
    [SerializeField] private string variableName = "doorCode";
    [SerializeField] private string expectedValue = "5023";

    [Header("Canvas Settings (optional)")]
    [SerializeField] private Canvas lumaChatCanvas;

    private bool lastUIState;

    private void Awake()
    {
        if (playButton) playButton.onClick.AddListener(OnPlayClicked);
        if (chatObject) chatObject.SetActive(false);
        if (lumaChatUI) lumaChatUI.SetActive(false);

        if (lumaChatUI && lumaChatCanvas == null)
            lumaChatCanvas = lumaChatUI.GetComponentInParent<Canvas>();

        if (lumaChatCanvas)
        {
            lumaChatCanvas.overrideSorting = true;
            lumaChatCanvas.sortingOrder = 50;
        }
    }

    private void OnDestroy()
    {
        if (playButton) playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void Update()
    {
        if (puzzleZone != null)
        {
            bool uiActive = puzzleZone.gameObject.activeInHierarchy;

            if (chatObject) chatObject.SetActive(uiActive);
            if (lumaChatUI) lumaChatUI.SetActive(uiActive);

            if (uiActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            lastUIState = uiActive;
        }
    }

    private void OnPlayClicked()
    {
        StartCoroutine(ValidateAfterDelay());
    }

    private System.Collections.IEnumerator ValidateAfterDelay()
    {
        yield return null;

        bool solved = CheckDoorPuzzle();

        if (solved)
        {
            if (doorController)
                doorController.OpenDoor();

            if (puzzleZone)
                puzzleZone.HideBlocksUI();

            if (chatObject)
                chatObject.SetActive(false);

            if (lumaChatUI)
                lumaChatUI.SetActive(false);
        }
        else
        {
            if (lumaChatUI)
            {
                lumaChatUI.SetActive(true);

                if (lumaChatCanvas)
                {
                    lumaChatCanvas.overrideSorting = true;
                    lumaChatCanvas.sortingOrder = 50;
                }

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private bool CheckDoorPuzzle()
    {
        if (executionManager == null)
            return false;

        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));

        foreach (var block in allBlocks)
        {
            if (block == null) continue;
            string name = block.name.ToLower();

            var inputs = GetInputs(block);
            if (inputs == null || inputs.Count == 0)
                inputs = new List<I_BE2_BlockSectionHeaderInput>(block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true));

            if (name.Contains("create") && name.Contains("int"))
            {
                if (inputs.Count >= 2)
                {
                    string varName = inputs[0].StringValue.Trim().ToLower();
                    string varValue = inputs[1].StringValue.Trim();

                    Debug.Log($"[Validator] Checking: {varName} = {varValue}");

                    if (varName == variableName.ToLower() && varValue == expectedValue)
                    {
                        Debug.Log($"[Validator] Puzzle solved. Matching block found: {varName} = {varValue}");
                        return true;
                    }
                }
            }
        }

        Debug.Log("[Validator] No matching block found.");
        return false;
    }

    private List<I_BE2_BlockSectionHeaderInput> GetInputs(BE2_Block block)
    {
        var inputs = new List<I_BE2_BlockSectionHeaderInput>();
        var type = block.GetType();

        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (typeof(IEnumerable<I_BE2_BlockSectionHeaderInput>).IsAssignableFrom(field.FieldType))
            {
                var val = field.GetValue(block);
                if (val is List<I_BE2_BlockSectionHeaderInput> list)
                    inputs.AddRange(list);
                else if (val is I_BE2_BlockSectionHeaderInput[] arr)
                    inputs.AddRange(arr);
            }
        }

        return inputs;
    }
}
