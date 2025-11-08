using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class WaterPumpPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private BlocksPuzzleZone puzzleZone;
    [SerializeField] private GameObject chatObject;
    [SerializeField] private GameObject lumaChatUI;
    [SerializeField] private WaterPumpController pumpController;

    [Header("Expected Values")]
    [SerializeField] private string variableName = "waterPressure";
    [SerializeField] private string expectedValue = "50.4";

    [Header("Canvas")]
    [SerializeField] private Canvas lumaChatCanvas;

    private void Awake()
    {
        if (playButton) playButton.onClick.AddListener(OnPlayClicked);
        if (chatObject) chatObject.SetActive(false);
        if (lumaChatUI) lumaChatUI.SetActive(false);
        if (lumaChatUI && lumaChatCanvas == null) lumaChatCanvas = lumaChatUI.GetComponentInParent<Canvas>();
        if (lumaChatCanvas) { lumaChatCanvas.overrideSorting = true; lumaChatCanvas.sortingOrder = 50; }
    }

    private void OnDestroy()
    {
        if (playButton) playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        StartCoroutine(ValidateAfterDelay());
    }

    private System.Collections.IEnumerator ValidateAfterDelay()
    {
        yield return null;

        bool solved = CheckPumpPuzzle();

        if (solved)
        { 
            if (pumpController) pumpController.ActivatePump(-.1f);
            if (puzzleZone) puzzleZone.HideBlocksUI();
            if (chatObject) chatObject.SetActive(false);
            if (lumaChatUI) lumaChatUI.SetActive(false);
        }
        else
        {
            if (lumaChatUI) lumaChatUI.SetActive(true);
            if (lumaChatCanvas) { lumaChatCanvas.overrideSorting = true; lumaChatCanvas.sortingOrder = 50; }
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private bool CheckPumpPuzzle()
    {
        if (executionManager == null)
            return false;

        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));

        foreach (var block in allBlocks)
        {
            if (block == null) continue;

            string name = block.name.ToLower();
            var inputs = block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true);

            if (name.Contains("create") && name.Contains("float"))
            {
                string varName = inputs[0].StringValue.Trim().ToLower();
                string varValue = inputs[1].StringValue.Trim();

                if (varName == "waterpressure" && varValue == "50.4")
                {
                    Debug.Log("[Validator] Puzzle solved: " + varName + " = " + varValue);
                    return true;
                }
            }
        }

        Debug.Log("[Validator] No matching block found.");
        return false;
    }


    private bool ValuesEqual(string a, string b)
    {
        if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var da) &&
            double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var db))
            return Mathf.Abs((float)(da - db)) < 0.0001f;

        return string.Equals(a.Trim(), b.Trim(), System.StringComparison.Ordinal);
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
                if (val is List<I_BE2_BlockSectionHeaderInput> list) inputs.AddRange(list);
                else if (val is I_BE2_BlockSectionHeaderInput[] arr) inputs.AddRange(arr);
            }
        }
        return inputs;
    }
}
