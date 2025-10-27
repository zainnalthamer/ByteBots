using MG_BlocksEngine2.Block;
using MG_BlocksEngine2.Block.Instruction;
using MG_BlocksEngine2.Core;
using UnityEngine;

public class BE2_Ins_SetPassword : BE2_InstructionBase, I_BE2_Instruction
{
    protected override void OnStart()
    {
        string passwordValue = "";

        if (Section0Inputs != null && Section0Inputs.Length > 0 && Section0Inputs[0] != null)
        {
            passwordValue = Section0Inputs[0].StringValue;
        }
        else
        {
            Debug.LogWarning("SetPassword block has no input — defaulting to empty string.");
        }

        PuzzleManager.Instance.SetPassword(passwordValue);

        GameObject.FindObjectOfType<BlocksUIAutoCloser>()?.CloseUIAfterRun();

        Debug.Log("Password set from BE2 block: " + passwordValue);

        ExecuteNextInstruction();
    }
}
