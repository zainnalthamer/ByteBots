using UnityEngine;
 
using MG_BlocksEngine2.Block.Instruction; 



public class BE2_Cst_CreateString : BE2_InstructionBase, I_BE2_Instruction

{

    public string blockStringName = "";
    public string blockStringValue = "";

    public void Function()

    {

        blockStringName = Section0Inputs[0].StringValue;
        blockStringValue = Section0Inputs[1].StringValue;

        Debug.LogWarning("Created String Variable: " + blockStringName + " with Value: " + blockStringValue);

    }

}


