using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;



public class BE2_Cst_CreateDouble : BE2_InstructionBase, I_BE2_Instruction

{

    public string blockDoubleName = "";
    public string blockDoubleValue = "";

    public void Function()

    {

        blockDoubleName = Section0Inputs[0].StringValue;
        blockDoubleValue = Section0Inputs[1].StringValue;

        Debug.LogWarning("Created double Variable: " + blockDoubleName + " with Value: " + blockDoubleValue);

        BE2_VariablesManager.instance.CreateAndAddVarToPanel(blockDoubleName);

    }

}


