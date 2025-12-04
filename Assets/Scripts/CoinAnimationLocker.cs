using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections; // Needed for the short delay

// This script needs an XRSimpleInteractable on the same GameObject (the Locking Cube)
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class CoinAnimationLocker : MonoBehaviour
{
    [Header("Target Coin")]
    [Tooltip("Drag the Coin GameObject whose animation you want to control.")]
    public GameObject targetCoinObject;

    // We get a reference to the animation component directly in the script
    private ComplexCoinAnimation coinAnimation;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    private void Start()
    {
        // Get the Interactable component on the Locking Cube
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // Find the ComplexCoinAnimation component on the target coin
        if (targetCoinObject != null)
        {
            coinAnimation = targetCoinObject.GetComponent<ComplexCoinAnimation>();
        }

        // IMPORTANT: Subscribe to the Hover events
        if (interactable != null)
        {
            interactable.firstHoverEntered.AddListener(OnHoverStart);
            interactable.lastHoverExited.AddListener(OnHoverEnd);
        }
    }

    /// <summary>
    /// Called when the gaze or controller ray starts hovering over the cube.
    /// </summary>
    private void OnHoverStart(HoverEnterEventArgs args)
    {
        if (coinAnimation != null)
        {
            // FREEZE THE COIN: Disabling the component stops the animation's Update loop.
            coinAnimation.enabled = false;
            Debug.Log("Coin animation LOCKED (Frozen).");
        }
    }

    /// <summary>
    /// Called when the gaze or controller ray leaves the cube.
    /// </summary>
    private void OnHoverEnd(HoverExitEventArgs args)
    {
        // We use a small delay (0.1s) to prevent accidental immediate re-enabling
        // if the ray briefly leaves and returns.
        StartCoroutine(EnableCoinAnimationAfterDelay(0.1f));
    }

    private IEnumerator EnableCoinAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (coinAnimation != null)
        {
            // UNFREEZE THE COIN: Enabling the component restarts the animation.
            coinAnimation.enabled = true;
            Debug.Log("Coin animation UNLOCKED (Bouncing).");
        }
    }
    
    // Clean up listeners
    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.firstHoverEntered.RemoveListener(OnHoverStart);
            interactable.lastHoverExited.RemoveListener(OnHoverEnd);
        }
    }
}
