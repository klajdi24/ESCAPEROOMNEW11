using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem; 

public class GazeInteractable : MonoBehaviour
{
    [Header("Gaze Settings")]
    [Tooltip("Time (in seconds) the player must gaze to activate.")]
    public float dwellTime = 5f;

    
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

    
    private bool isHovered = false;
    private bool isActivated = false; 
    private float dwellTimer = 0f;

    
    void OnEnable()
    {
        
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
    

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend) originalColor = rend.material.color;
    }

    void Update()
    {
        if (isHovered && !isActivated)
        {
           
            
            if (primaryInteractionAction.action != null && 
                primaryInteractionAction.action.ReadValue<float>() > 0.1f)
            {
                OnGazeActivate();
                return; 
            }

            
            dwellTimer += Time.deltaTime;

            if (dwellTimer >= dwellTime)
            {
                OnGazeActivate();
                
            }
        }
    }

    
    public void OnGazeEnter()
    {

        Debug.Log("hover");
        if (isHovered) return;

        isHovered = true;
        isActivated = false;
        dwellTimer = 0f;
        onHoverEnter?.Invoke();
        if (rend) rend.material.color = hoverColor;
    }

    
    public void OnGazeExit()
    {


        Debug.Log("exit");
        isHovered = false;
        isActivated = false;
        dwellTimer = 0f;

        onHoverExit?.Invoke();
        if (rend) rend.material.color = originalColor;
    }

    
    public void OnGazeActivate()
    {


        Debug.Log("active");
        if (isActivated) return;

        isActivated = true;
        onActivate?.Invoke(); 
    }
}

