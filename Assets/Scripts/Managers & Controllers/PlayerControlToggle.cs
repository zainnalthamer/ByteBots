using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerControlToggle : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs starterInputs;
    [SerializeField] private PlayerInput playerInput;

    public void DisableControl()
    {
        if (starterInputs) starterInputs.enabled = false;
        if (playerInput) playerInput.enabled = false;
    }

    public void EnableControl()
    {
        if (playerInput) playerInput.enabled = true;
        if (starterInputs) starterInputs.enabled = true;
    }
}
