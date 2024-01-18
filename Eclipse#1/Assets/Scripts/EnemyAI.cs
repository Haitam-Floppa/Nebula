using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 5f;
    public Transform[] waypoints;
    public float moveSpeed = 2f;

    private Transform player;
    private HealthBar healthBar; // Reference to the HealthBar component
    public GameObject healthBarPrefab;

    public int maxHealth = 100;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Instantiate and set up health bar
        GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarObject.transform.SetParent(transform);
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);

        PatrolWaypoints();
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            // Follow the player if detected
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Patrol waypoints if player is not detected
            PatrolWaypoints();
        }
    }

    private bool CanSeePlayer()
    {
        // Check if the player is within the detection radius
        return Vector2.Distance(transform.position, player.position) < detectionRadius;
    }

    private void PatrolWaypoints()
    {
        // Implement logic to patrol between waypoints
    }
}
