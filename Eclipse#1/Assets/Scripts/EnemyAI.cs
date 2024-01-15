using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    private Transform player;
    private int currentWaypointIndex = 0;

    // Reference to the detection range GameObject
    public GameObject detectionRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy! Assign waypoints in the Unity Editor.");
            enabled = false;
        }

        if (detectionRange == null)
        {
            Debug.LogError("Detection range GameObject not assigned! Assign a GameObject in the Unity Editor.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints.Length == 0) return;

        MoveTowardsWaypoint();
        CheckWaypointReached();
    }

    private void MoveTowardsWaypoint()
    {
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, step);
    }

    private void CheckWaypointReached()
    {
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            float step = chaseSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, player.position, step);
        }
    }

    private bool CanSeePlayer()
    {
        // Ensure the detection range GameObject is set
        if (detectionRange == null) return false;

        // Check if the player is within the detection range
        return detectionRange.GetComponent<DetectionRange>().IsPlayerDetected();
    }
}
