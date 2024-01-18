using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private HealthBar healthBar; // Reference to the HealthBar component
    public GameObject healthBarPrefab;

    private void Start()
    {
        currentHealth = maxHealth;

        // Instantiate and set up health bar
        GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarObject.transform.SetParent(transform);
        healthBar = healthBarObject.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Ensure health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0);

        // Update the health bar with the current health value
        healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Implement death behavior here (e.g., play death animation, spawn particles, etc.)
        Destroy(gameObject);
    }
}
