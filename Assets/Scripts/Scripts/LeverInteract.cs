using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LeverInteract : MonoBehaviour
{
    public LeverScript leverScript;
    public string playerTag = "Player";
    public float interactDistance = 2f;
    private Transform player;
    private bool playerInRange = false;

    void Start()
    {
        if (leverScript == null)
            leverScript = GetComponent<LeverScript>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLever();
        }
    }

    void ToggleLever()
    {
        if (leverScript == null) return;

        leverScript.LeverState = !leverScript.LeverState;

        if (leverScript.UseSoundEffects)
            leverScript.GetComponent<AudioSource>()?.Play();

        if (leverScript.UseAnimations && leverScript.StickGameObject != null)
        {
            Animator anim = leverScript.StickGameObject.GetComponent<Animator>();
            anim.Play(leverScript.LeverState ? "LeverOnAnimation" : "LeverOffAnimation");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = other.transform;
            playerInRange = true;
            Debug.Log("Player in range — Press E to use lever");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            Debug.Log("Player left lever range");
        }
    }
}
