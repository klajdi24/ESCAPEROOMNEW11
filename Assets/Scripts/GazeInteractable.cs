using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; 

public class GazeInteractable : MonoBehaviour
{
    [Header("Gaze Settings")]
    [Tooltip("Time (in seconds) the player must gaze to activate.")]
    public float dwellTime = 1.2f;

    // The Inspector reference is BACK! You must link this in the Inspector.
    [Header("Controller Settings")]
    [Tooltip("The Input Action to check for immediate activation (e.g., Trigger Press)")]
    public InputActionProperty primaryInteractionAction; 
    
    [Header("Events")]
    public UnityEvent onActivate;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    [Header("Visual Feedback")]
    private Renderer rend;
    private Color originalColor;
    public Color hoverColor = Color.yellow;

    // State variables
    private bool isHovered = false;
    private bool isActivated = false; 
    private float dwellTimer = 0f;

    // 🔑 THE FIX: Enables and disables the controller action linked in the Inspector. 🔑
    void OnEnable()
    {
        // This is necessary to start listening for the trigger press.
        if (primaryInteractionAction.action != null)
        {
            primaryInteractionAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (primaryInteractionAction.action != null)
        {
            primaryInteractionAction.action.Disable();
        }
    }
    // 🔑 END FIX 🔑

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend) originalColor = rend.material.color;
    }

    void Update()
    {
        if (isHovered && !isActivated)
        {
            // 1. Controller Activation (Instantaneous)
            // Checks if the primary interaction button is pressed while hovering
            if (primaryInteractionAction.action != null && 
                primaryInteractionAction.action.ReadValue<float>() > 0.1f)
            {
                OnGazeActivate();
                return; // Exit Update to prevent further gaze processing
            }

            // 2. Gaze Dwell Activation (Timed)
            dwellTimer += Time.deltaTime;

            if (dwellTimer >= dwellTime)
            {
                OnGazeActivate();
            }
        }
    }

    // Called by the VR system when the ray hits this object (Hover/Color Change)
    public void OnGazeEnter()
    {
        if (isHovered) return;

        isHovered = true;
        isActivated = false;
        dwellTimer = 0f;
        onHoverEnter?.Invoke();
        if (rend) rend.material.color = hoverColor;
    }

    // Called by the VR system when the ray leaves this object
    public void OnGazeExit()
    {
        isHovered = false;
        isActivated = false;
        dwellTimer = 0f;

        onHoverExit?.Invoke();
        if (rend) rend.material.color = originalColor;
    }

    // Activated by either the Update loop (Gaze Dwell) or Controller Check
    public void OnGazeActivate()
    {
        if (isActivated) return;

        isActivated = true;
        onActivate?.Invoke(); 
    }
}

