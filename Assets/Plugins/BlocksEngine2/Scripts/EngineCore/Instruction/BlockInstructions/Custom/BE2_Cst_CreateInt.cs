using UnityEngine;

using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Environment;



public class BE2_Cst_CreateInt : BE2_InstructionBase, I_BE2_Instruction

{

    public string blockIntName = "";
    public string blockIntValue = "";

    public void Function()

    {

        blockIntName = Section0Inputs[0].StringValue;
        blockIntValue = Section0Inputs[1].StringValue;

        Debug.LogWarning("Created int Variable: " + blockIntName + " with Value: " + blockIntValue);

        BE2_VariablesManager.instance.CreateAndAddVarToPanel(blockIntName);

    }

}


