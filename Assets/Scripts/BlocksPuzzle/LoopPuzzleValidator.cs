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

    private void Awake()
    {
        if (playButton) playButton.onClick.AddListener(OnPlayClicked);
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
        bool solved = CheckLoopPuzzle();

        if (solved)
        {
            Debug.Log("[LoopPuzzleValidator] Correct program!");
            if (rideToRotate)
            {
                rideToRotate.enabled = true;
                Debug.Log("[LoopPuzzleValidator] Rotation script enabled.");
            }
            if (puzzleZone)
            {
                puzzleZone.HideBlocksUI();
            }
        }
        else
        {
            Debug.Log("[LoopPuzzleValidator] Incorrect program - must be: repeat 3 times -> rotate Y 30 degrees");
        }
    }

    private bool CheckLoopPuzzle()
    {
        if (executionManager == null)
        {
            Debug.LogError("ExecutionManager not assigned!");
            return false;
        }

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
            {
                inputs = new List<I_BE2_BlockSectionHeaderInput>(block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true));
            }

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

        Debug.Log($"[Validator Summary] Repeat={repeatCount}, Axis={axis}, Degrees={degrees}");

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
