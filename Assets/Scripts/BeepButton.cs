using UnityEngine;

public class BeepButton : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayBeep()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
