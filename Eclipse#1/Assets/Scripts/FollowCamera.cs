using UnityEngine;

public class DynamicCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minZoom = 2.3f;
    public float maxZoom = 3f;
    public float zoomLerpSpeed = 5f;
    public float followDistanceThreshold = 1.0f;
    public float deadZone = 1.0f; // Introduce a dead zone or buffer

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (mainCamera.orthographic == false)
        {
            Debug.LogError("Please set the camera projection to Orthographic.");
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogError("Target not set for the DynamicCameraFollow script!");
            return;
        }

        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;

        // Calculate the distance between the current position and the target position
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // Only follow the target if it has moved beyond the followDistanceThreshold
        if (distance > followDistanceThreshold)
        {
            // Calculate a new position with a dead zone or buffer
            Vector3 newPosition = CalculateNewPosition(targetPosition, currentPosition, deadZone);

            // Smoothly follow the target
            Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }

        // Calculate zoom based on player speed
        float speedFactor = Mathf.Clamp(target.GetComponent<Rigidbody2D>().velocity.magnitude / 5f, 0f, 1f);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speedFactor);

        // Smoothly adjust the camera's orthographic size
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }

    Vector3 CalculateNewPosition(Vector3 targetPosition, Vector3 currentPosition, float deadZone)
    {
        // Calculate the direction vector from the current position to the target position
        Vector3 direction = targetPosition - currentPosition;

        // Calculate the distance between the current position and the target position
        float distance = direction.magnitude;

        // If the distance is greater than the dead zone, move towards the target
        if (distance > deadZone)
        {
            float ratio = (distance - deadZone) / distance;
            return currentPosition + direction * ratio;
        }
        else
        {
            // If the distance is within the dead zone, stay at the current position
            return currentPosition;
        }
    }
}
