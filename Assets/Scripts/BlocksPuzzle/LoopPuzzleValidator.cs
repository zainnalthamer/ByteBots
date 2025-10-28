using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Core;
using MG_BlocksEngine2.Environment;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class LoopPuzzleValidator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BE2_ExecutionManager executionManager;
    [SerializeField] private Button playButton;
    [SerializeField] private MonoBehaviour rideToRotate;
    [SerializeField] private BlocksPuzzleZone puzzleZone;
    [SerializeField] private GameObject chatObject;
    [SerializeField] private GameObject lumaChatUI;

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

        bool solved = CheckLoopPuzzle();

        if (solved)
        {
            if (rideToRotate)
                rideToRotate.enabled = true;

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

    private bool CheckLoopPuzzle()
    {
        if (executionManager == null)
            return false;

        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));

        BE2_Block repeatBlock = null;
        BE2_Block rotateBlock = null;
        int repeatCount = 0;
        string axis = "";
        float degrees = 0f;

        foreach (var block in allBlocks)
        {
            if (block == null) continue;
            string name = block.name.ToLower();

            var inputs = GetInputs(block);
            if (inputs == null || inputs.Count == 0)
                inputs = new List<I_BE2_BlockSectionHeaderInput>(block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true));

            if (name.Contains("repeat") || name.Contains("loop"))
            {
                repeatBlock = block;
                if (inputs.Count > 0)
                {
                    string val = inputs[0].StringValue;
                    int.TryParse(val, out repeatCount);
                }
            }

            if (name.Contains("rotate"))
            {
                rotateBlock = block;
                if (inputs.Count >= 2)
                {
                    axis = inputs[0].StringValue.ToLower();
                    float.TryParse(inputs[1].StringValue, out degrees);
                }
            }
        }

        bool hasRepeat = repeatBlock != null && repeatCount == 3;
        bool hasRotate = rotateBlock != null && axis.Contains("y") && Mathf.Approximately(degrees, 30f);

        return hasRepeat && hasRotate;
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
