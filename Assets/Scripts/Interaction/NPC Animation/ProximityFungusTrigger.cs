using UnityEngine;
using Fungus;

public class ProximityFungusTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public Transform player;
    public float triggerDistance = 3f;

    [Header("Quest")]
    public int requiredQuestIndex;

    [Header("Fungus")]
    public Flowchart flowchart;
    public string blockName;

    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered || player == null || flowchart == null)
            return;

        if (!QuestManager.Instance ||
            !QuestManager.Instance.IsCurrentQuest(requiredQuestIndex))
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
        {
            hasTriggered = true;
            flowchart.ExecuteBlock(blockName);
        }
    }
}
