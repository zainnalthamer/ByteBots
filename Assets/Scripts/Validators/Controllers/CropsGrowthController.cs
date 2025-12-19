using DG.Tweening;
using UnityEngine;

public class CropsGrowthController : MonoBehaviour
{
    [Header("References")]
    public Transform cropsParent;
    public AudioSource growthAudio;

    [Header("Animation")]
    [Range(0.1f, 5f)] public float growDuration = 2.5f;
    public float targetY = 38.98f;

    public void GrowCrops()
    {
        if (growthAudio) growthAudio.Play();

        if (!cropsParent)
        {
            Debug.LogWarning("[CropsGrowthController] No cropsParent assigned!");
            return;
        }

        var p = cropsParent.localPosition;
        cropsParent.DOLocalMoveY(targetY, growDuration).SetEase(Ease.InOutBounce);
        Debug.Log($"[CropsGrowthController] Growing crops to Y={targetY}");
    }
}