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
    public float spinSpeed = 180.0f; 

    private Vector3 startPosition;

    void Start()
    {
        
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float time = Time.time * movementSpeed;
        
        

        
        float xOffset = Mathf.Sin(time) * horizontalRadius;

        
        float zOffset = Mathf.Cos(time * 0.7f) * horizontalRadius; 

        
        float yOffset = Mathf.Sin(time * 2.0f) * verticalAmplitude; 

        
        transform.localPosition = new Vector3(
            startPosition.x + xOffset,
            startPosition.y + yOffset,
            startPosition.z + zOffset
        );

        

        
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
    }
}
