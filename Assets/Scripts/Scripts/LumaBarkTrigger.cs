using UnityEngine;
using Fungus;

public class LumaBarkTrigger : MonoBehaviour
{
    public Flowchart flowchart;
    public string blockName = "Bark";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && flowchart != null)
        {
            flowchart.ExecuteBlock(blockName);
        }
    }
}