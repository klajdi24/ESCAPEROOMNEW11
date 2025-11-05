using UnityEngine;
using UnityEngine.Events;

public class GazeInteractable : MonoBehaviour
{
    public UnityEvent onActivate;
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    private Renderer rend;
    private Color originalColor;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend) originalColor = rend.material.color;
    }

    public void OnGazeEnter()
    {
        onHoverEnter?.Invoke();
        if (rend) rend.material.color = hoverColor;
    }

    public void OnGazeExit()
    {
        onHoverExit?.Invoke();
        if (rend) rend.material.color = originalColor;
    }

    public void OnGazeActivate()
    {
        onActivate?.Invoke();
    }
}

