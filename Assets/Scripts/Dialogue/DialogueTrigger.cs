using UnityEngine;
using Fungus;

public class DialogueTrigger : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flowchart.ExecuteBlock(blockName);
        }
    }
}
