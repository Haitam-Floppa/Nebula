using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Player's transform
    public float smoothSpeed = 0.125f;
    public float horizontalOffset = 2.0f;
    public float verticalOffset = 1.0f;
    public LayerMask groundLayer; // Set this layer to the one representing the ground

    private float currentHorizontalOffset;
    private bool isGrounded;

    void Start()
    {
        currentHorizontalOffset = horizontalOffset;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            float targetHorizontalOffset = target.right.x * horizontalOffset;

            // Adjust horizontal offset smoothly based on player's direction
            if (target.localScale.x < 0) // player facing left
            {
                targetHorizontalOffset *= -1.0f;
            }

            // Check if the player is grounded before applying vertical offset
            isGrounded = Physics.Raycast(target.position, Vector3.down, 0.1f, groundLayer);

            // Gradually interpolate the horizontal offset
            currentHorizontalOffset = Mathf.Lerp(currentHorizontalOffset, targetHorizontalOffset, smoothSpeed);

            // Apply vertical offset only if the player is grounded
            float targetVerticalOffset = isGrounded ? verticalOffset : 0f;

            Vector3 targetPosition = new Vector3(target.position.x + currentHorizontalOffset, target.position.y + targetVerticalOffset, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
