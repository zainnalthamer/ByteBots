using DG.Tweening;
using UnityEngine;

public class WaterPumpController : MonoBehaviour
{
    public Transform pondWater;
    public AudioSource pumpAudio;
     

    public void ActivatePump(float waterHeight)
    {
        Debug.Log("[PumpController] Activating pump...");

        if (pumpAudio)
            pumpAudio.Play();

        if (pondWater)
        {
            //pondWater.localPosition = targetPosition;
            pondWater.transform.DOLocalMoveY(waterHeight, 2.0f).SetEase(Ease.InOutSine);
            Debug.Log($"[PumpController] Water moved to {waterHeight} on Y axis");
        }
        else
        {
            Debug.LogWarning("[PumpController] No pond water assigned!");
        }
    }
}
