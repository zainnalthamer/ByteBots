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

    [Header("Puzzle Card Settings")]
    [SerializeField] private PuzzleCardController puzzleCard;
    [TextArea(2, 4)]
    [SerializeField]
    private string puzzleQuestion =
        "Use a loop that repeats 3 times and rotates the ride 30° on the Y axis.";
    [SerializeField] private Sprite lumaIcon;

    [Header("Choice Card Settings")]
    [SerializeField] private ChoiceCardController choiceCard;
    [SerializeField] private string conceptName = "loops";

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

        if(Input.GetKeyDown(KeyCode.O))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }





        if (puzzleZone != null)
        {
            bool uiActive = puzzleZone.gameObject.activeInHierarchy;

            if (chatObject)
                chatObject.SetActive(uiActive);

            if (uiActive && !lastUIState)
            {
                if (puzzleCard)
                    puzzleCard.Show(puzzleQuestion, lumaIcon);

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

            if (puzzleCard)
                puzzleCard.Hide();

            if (choiceCard)
                choiceCard.Hide();
        }
        else
        {
            if (puzzleCard)
                puzzleCard.Show(puzzleQuestion, lumaIcon);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private bool CheckLoopPuzzle()
    {
        if (executionManager == null)
            return false;

        var allBlocks = new List<BE2_Block>(
            executionManager.GetComponentsInChildren<BE2_Block>(true));

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
                inputs = new List<I_BE2_BlockSectionHeaderInput>(
                    block.GetComponentsInChildren<I_BE2_BlockSectionHeaderInput>(true));

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
        bool hasRotate =
            rotateBlock != null && axis.Contains("y") && Mathf.Approximately(degrees, 30f);

        return hasRepeat && hasRotate;
    }
    
    private List<I_BE2_BlockSectionHeaderInput> GetInputs(BE2_Block block)
    {
        var inputs = new List<I_BE2_BlockSectionHeaderInput>();
        var type = block.GetType();

        foreach (var field in type.GetFields(
                     BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleCard)
                puzzleCard.Show(puzzleQuestion, lumaIcon);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puzzleCard)
                puzzleCard.Hide();
            if (choiceCard)
                choiceCard.Hide();
        }
    }

    public void OnLumaIconClicked()
    {
        if (puzzleCard)
            puzzleCard.Hide();

        if (choiceCard)
            choiceCard.Show(conceptName, lumaIcon);
    }
}
