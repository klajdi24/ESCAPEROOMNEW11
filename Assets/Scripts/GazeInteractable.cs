using UnityEngine;
using UnityEngine.Events;

public class GazeInteractable : MonoBehaviour
{
    public UnityEvent onActivate;

    public void OnGazeEnter()  { /* optional: highlight */ }
    public void OnGazeExit()   { /* optional: remove highlight */ }
    public void OnGazeActivate() => onActivate?.Invoke();
}

