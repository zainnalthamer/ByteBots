using UnityEngine;

public class BE2InitializerFix : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(ReinitializeBE2), 1f);
    }

    void ReinitializeBE2()
    {
        var execution = FindObjectOfType<MG_BlocksEngine2.Core.BE2_ExecutionManager>();
        if (execution != null)
        {
            Debug.Log("[BE2InitializerFix] Reinitializing BE2 Execution Manager...");

            execution.enabled = false;
            execution.enabled = true;
        }
        else
        {
            Debug.LogWarning("[BE2InitializerFix] No BE2_ExecutionManager found!");
        }
    }
}
