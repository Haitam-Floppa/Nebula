using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;        // Reference to the Slider component
    public Transform target;     // Reference to the target (enemy) to follow

    private void Start()
    {
        // Ensure that the Slider component is assigned in the Unity Editor
        if (slider == null)
        {
            Debug.LogError("Slider component is not assigned for the HealthBar script.");
            return;
        }

        // Ensure that the Target is assigned in the Unity Editor
        if (target == null)
        {
            Debug.LogError("Target (enemy) is not assigned for the HealthBar script.");
            return;
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        slider.value = currentHealth;

        // Ensure the health bar follows the target
        if (target != null)
        {
            transform.position = target.position + new Vector3(0, 2, 0); // Adjust the offset as needed
        }

        // Uncomment the following line if needed
        // slider.value = Mathf.Clamp(slider.value, slider.minValue, slider.maxValue);
    }
}
