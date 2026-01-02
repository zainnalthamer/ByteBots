using UnityEngine;
using Fungus;

public class NPCClickHandler : MonoBehaviour
{
    [Header("Fungus")]
    public Flowchart flowchart;
    public string blockName;

    private void OnMouseDown()
    {
        if (flowchart == null)
        {
            Debug.LogWarning($"{name}: No Flowchart assigned!");
            return;
        }

        if (string.IsNullOrEmpty(blockName))
        {
            Debug.LogWarning($"{name}: No blockName assigned!");
            return;
        }

        flowchart.ExecuteBlock(blockName);
    }
}
