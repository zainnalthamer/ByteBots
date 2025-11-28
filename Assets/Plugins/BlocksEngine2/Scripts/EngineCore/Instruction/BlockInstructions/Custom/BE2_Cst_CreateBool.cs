using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;



public class BE2_Cst_CreateBool : BE2_InstructionBase, I_BE2_Instruction

{

    public string blockBoolName = "";
    public string blockBoolValue = "";

    public void Function()

    {

        blockBoolName = Section0Inputs[0].StringValue;
        blockBoolValue = Section0Inputs[1].StringValue;

        Debug.LogWarning("Created bool Variable: " + blockBoolName + " with Value: " + blockBoolValue);

        BE2_VariablesManager.instance.CreateAndAddVarToPanel(blockBoolName);

    }

}


