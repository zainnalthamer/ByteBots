using UnityEngine;

public class AreaEnvironmentSoundTrigger : MonoBehaviour
{
    [Header("Environment Sound")]
    [Tooltip("Index from SoundController.environmentClips")]
    public int environmentClipIndex;

    [Header("Fade Settings")]
    public float fadeInDuration = 2.5f;
    public float fadeOutDuration = 2.5f;
    [Range(0f, 1f)] public float targetVolume = 0.55f;



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered environment sound trigger: " + environmentClipIndex);

            SoundController.Instance.PlayOrSwitchEnvironment(environmentClipIndex);
            //SoundController.Instance.FadeEnvironment(fadeInDuration, targetVolume);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            SoundController.Instance.FadeEnvironment(fadeOutDuration, 0f);
        }
    }
}