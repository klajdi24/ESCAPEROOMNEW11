using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GazeInteractor : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask interactableLayer = ~0; // all layers by default
    public float dwellTime = 1.2f;
    public Transform reticle;

    Camera cam;
    GazeInteractable current;
    float hoverTimer = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            var gi = hit.collider.GetComponent<GazeInteractable>();
            if (gi != null)
            {
                if (current != gi)
                {
                    current?.OnGazeExit();
                    current = gi;
                    current.OnGazeEnter();
                    hoverTimer = 0f;
                }
                else
                {
                    hoverTimer += Time.deltaTime;
                    if (hoverTimer >= dwellTime)
                    {
                        current.OnGazeActivate();
                        hoverTimer = 0f;
                    }
                }
                // optionally move reticle to hit point
                if (reticle != null)
                {
                    reticle.position = hit.point;
                }
                return;
            }
        }

        // nothing hit
        if (current != null)
        {
            current.OnGazeExit();
            current = null;
        }
        hoverTimer = 0f;
    }
}

