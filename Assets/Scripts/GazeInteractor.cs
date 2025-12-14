using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GazeInteractor : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 10f;
    public LayerMask interactableLayer = ~0;

    [Header("Gaze Timing")]
    public float dwellTime = 1.2f;
    public float hoverGraceTime = 0.15f; // 👈 NEW (important)

    [Header("Visuals")]
    public Transform reticle;

    private Camera cam;
    private GazeInteractable current;
    private float hoverTimer = 0f;
    private float graceTimer = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            var gi = hit.collider.GetComponent<GazeInteractable>();

            if (gi != null)
            {
                // Reset grace timer because we have a valid hit
                graceTimer = 0f;

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
                        hoverTimer = -999f; // lock activation
                    }
                }

                if (reticle != null)
                {
                    reticle.position = hit.point;
                }

                return;
            }
        }

        // ❗ No hit this frame — start grace period instead of instant reset
        if (current != null)
        {
            graceTimer += Time.deltaTime;

            if (graceTimer >= hoverGraceTime)
            {
                current.OnGazeExit();
                current = null;
                hoverTimer = 0f;
            }
        }
    }
}



