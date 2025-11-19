using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GazeInteractor : MonoBehaviour
{
    public float maxDistance = 10f;
    public LayerMask interactableLayer = ~0; // all layers by default
    public float dwellTime = 1.2f;
    public Transform reticle;

    private Camera cam;
    private GazeInteractable current;
    private float hoverTimer = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.green);

        // 🔍 Cast the ray
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
         //   Debug.Log("Ray hit: " + hit.collider.gameObject.name);

            var gi = hit.collider.GetComponent<GazeInteractable>();

            if (gi != null)
            {
                // if we just started looking at a new object
                if (current != gi)
                {
                    current?.OnGazeExit();
                    current = gi;
                    current.OnGazeEnter();
                    hoverTimer = 0f;
                }
                else
                {
                    // we are still looking at the same object
                    hoverTimer += Time.deltaTime;
                    if (hoverTimer >= dwellTime)
                    {
                        current.OnGazeActivate();
                        hoverTimer = 0f;
                    }
                }

                // move the reticle to where the ray hits
                if (reticle != null)
                {
                    reticle.position = hit.point;
                }

                // early exit so we don't reset everything
                return;
            }
        }

        // nothing hit — reset
        if (current != null)
        {
            current.OnGazeExit();
            current = null;
        }

        hoverTimer = 0f;
    }
}


