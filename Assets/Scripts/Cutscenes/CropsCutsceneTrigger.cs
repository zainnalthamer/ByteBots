using UnityEngine;

public class CropsCutsceneTrigger : MonoBehaviour
{
    public Transform plantingSpot;
    public Animator patchAnimator;
    public Fungus.Flowchart flowchart;
    public string dialogueBlockName;
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.2f;

    public bool cutsceneStarted = false;
    public Transform player;
    CharacterController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<CharacterController>();
    } 

    void OnTriggerEnter(Collider other)
    {
        if (!cutsceneStarted && other.CompareTag("Player"))
        {
            cutsceneStarted = true;
             
            playerController.enabled = false;

            StartCoroutine(PlayCutscene());

            QuestManager.Instance.OnPuzzleCompleted(2);
        }
    }

    private System.Collections.IEnumerator PlayCutscene()
    {
        //while (Vector3.Distance(player.position, plantingSpot.position) > stoppingDistance)
        //{
        //    player.position = Vector3.MoveTowards(
        //        player.position,
        //        plantingSpot.position,
        //        moveSpeed * Time.deltaTime
        //    );
        //    yield return null;
        //}

        player.LookAt(plantingSpot);

        patchAnimator.SetTrigger("Plant");

        yield return new WaitForSeconds(4f);

        flowchart.ExecuteBlock(dialogueBlockName);

        flowchart.SetBooleanVariable("CutsceneDone", true);

        playerController.enabled = true;
    }
}
