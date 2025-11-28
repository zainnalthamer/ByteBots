using UnityEngine;
using System.Collections.Generic;
using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;
using MG_BlocksEngine2.Utils;

public class BE2_Cst_VariableWithDT : BE2_InstructionBase
{
    public static Dictionary<string, object> customVariables = new Dictionary<string, object>();

    public void Function()
    {
        string variableName = Section0Inputs[0].StringValue;
        string dataType = Section0Inputs[1].StringValue;
        string valueText = Section0Inputs[2].StringValue;

        object finalValue = valueText;

        switch (dataType)
        {
            case "int":
                if (int.TryParse(valueText, out int i)) finalValue = i;
                break;
            case "float":
                if (float.TryParse(valueText, out float f)) finalValue = f;
                break;
            case "bool":
                if (bool.TryParse(valueText, out bool b)) finalValue = b;
                break;
        }

        if (customVariables.ContainsKey(variableName))
            customVariables[variableName] = finalValue;
        else
            customVariables.Add(variableName, finalValue);

        Debug.Log($"Set variable '{variableName}' ({dataType}) = {finalValue}");

        ExecuteNextInstruction();
    }

    public static object GetVariable(string variableName)
    {
        if (customVariables.ContainsKey(variableName))
            return customVariables[variableName];
        return null;
    }
}
