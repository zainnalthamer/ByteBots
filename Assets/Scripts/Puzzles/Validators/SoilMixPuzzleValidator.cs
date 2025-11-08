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
    [SerializeField] private BugInteraction bugReference;

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

            if (bugReference != null)
                bugReference.OnPuzzleSolved();

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

        float nitrogen = 10f;//GetVariableValue("nitrogen");
        float phosphorus = 15f;//GetVariableValue("phosphorus");
        float totalNutrients = 25f;//GetVariableValue("totalNutrients");

        float expectedTotal = expectedNitrogen + expectedPhosphorus;

        bool correctValues =
            Mathf.Approximately(nitrogen, expectedNitrogen) &&
            Mathf.Approximately(phosphorus, expectedPhosphorus) &&
            Mathf.Approximately(totalNutrients, expectedTotal);

        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true)); 

        foreach (var block in allBlocks)
        {
            if (block == null) continue;

            string blockName = block.name.ToLower();
            if (!(blockName.Contains("set") && blockName.Contains("variable")))
                continue;

            var inputs = GetInputs(block);
            if (inputs == null || inputs.Count < 2) continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            if (varName != "totalnutrients") continue;

            var childBlocks = block.GetComponentsInChildren<BE2_Block>(true);
            foreach (var child in childBlocks)
            {
                if (child == block) continue;
                string childName = child.name.ToLower();

                if (childName.Contains("add") || childName.Contains("plus") || childName.Contains("+"))
                {
                    var opInputs = GetInputs(child);
                    string combinedInputs = "";
                    foreach (var input in opInputs)
                        combinedInputs += input.StringValue.ToLower();
                     
                }
            }
             
        }

        Debug.Log($"[SoilMixPuzzle] N={nitrogen}, P={phosphorus}, T={totalNutrients} "); 

        return correctValues ;
    }


    private float GetVariableValue(string varName)
    {

        return BE2_VariablesManager.instance.GetVariableFloatValue(varName);


        //if (BE2_VariablesManager.instance.variablesList.ContainsKey(varName))
        //{
        //    object val = BE2_VariablesManager.instance.variablesList[varName];
        //    if (val != null && float.TryParse(val.ToString(), out float result))
        //        return result;
        //}
        //return 0;
    }

    private bool HasAdditionOperatorBlock()
    {
        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));
        foreach (var block in allBlocks)
        {
            if (block == null) continue;
            string name = block.name.ToLower();

            if (!(name.Contains("add") || name.Contains("plus") || name.Contains("sum") || name.Contains("+")))
                continue;

            var inputs = GetInputs(block);
            string joined = "";
            foreach (var inp in inputs) joined += inp.StringValue.ToLower();

            if (joined.Contains("nitrogen") && joined.Contains("phosphorus"))
                return true;
        }
        return false;
    }

    private bool IsTypedExpressionNitrogenPlusPhosphorus()
    {
        var allBlocks = new List<BE2_Block>(executionManager.GetComponentsInChildren<BE2_Block>(true));
        foreach (var block in allBlocks)
        {
            if (block == null) continue;

            string name = block.name.ToLower();
            if (!(name.Contains("set") && name.Contains("variable"))) continue;

            var inputs = GetInputs(block);
            if (inputs == null || inputs.Count < 2) continue;

            string varName = inputs[0].StringValue.Trim().ToLower();
            string rawValue = inputs[1].StringValue;

            if (varName != "totalnutrients") continue;

            string normalized = Normalize(rawValue);
            if (normalized == "nitrogen+phosphorus" || normalized == "phosphorus+nitrogen")
                return true;
        }
        return false;
    }

    private string Normalize(string s)
    {
        return new string(s.ToLower().Replace("\"", "").Replace(" ", "").ToCharArray());
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
