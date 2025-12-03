using UnityEngine;

public class ComplexCoinAnimation : MonoBehaviour
{
    [Header("Positional Oscillation")]
    [Tooltip("The radius of the circular horizontal movement (X and Z axes).")]
    public float horizontalRadius = 0.15f; 

    [Tooltip("The vertical bouncing amplitude (Y-axis).")]
    public float verticalAmplitude = 0.08f; 

    [Tooltip("The speed of the overall position movement.")]
    public float movementSpeed = 1.5f; 

    [Header("Rotation")]
    [Tooltip("The speed at which the coin spins around the Y-axis.")]
    public float spinSpeed = 180.0f; // Degrees per second

    private Vector3 startPosition;

    void Start()
    {
        // Store the coin's original position when the game starts.
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float time = Time.time * movementSpeed;
        
        // --- 1. Calculate Positional Movement ---

        // X-Axis (Oscillation): Uses sine for back-and-forth movement, based on time.
        float xOffset = Mathf.Sin(time) * horizontalRadius;

        // Z-Axis (Oscillation): Uses cosine for complementary movement, creating a circular or elliptical path.
        float zOffset = Mathf.Cos(time * 0.7f) * horizontalRadius; // Use 0.7f for slightly non-circular motion

        // Y-Axis (Bouncing): Uses a faster sine wave for the vertical bounce.
        float yOffset = Mathf.Sin(time * 2.0f) * verticalAmplitude; // Bounces twice as fast as horizontal movement

        // Set the new, complex position relative to the starting point.
        transform.localPosition = new Vector3(
            startPosition.x + xOffset,
            startPosition.y + yOffset,
            startPosition.z + zOffset
        );

        // --- 2. Calculate Rotation ---

        // Rotate the coin around its local Y-axis continuously.
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
    }
}
