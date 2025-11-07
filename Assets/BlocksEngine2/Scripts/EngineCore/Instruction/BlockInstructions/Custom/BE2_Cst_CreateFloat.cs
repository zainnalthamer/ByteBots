using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;



public class BE2_Cst_CreateFloat : BE2_InstructionBase, I_BE2_Instruction

{

    public string blockFloatName = "";
    public string blockFloatValue = "";

    public void Function()

    {

        blockFloatName = Section0Inputs[0].StringValue;
        blockFloatValue = Section0Inputs[1].StringValue;

        Debug.LogWarning("Created float Variable: " + blockFloatName + " with Value: " + blockFloatValue);

        BE2_VariablesManager.instance.CreateAndAddVarToPanel(blockFloatName);

    }

}


