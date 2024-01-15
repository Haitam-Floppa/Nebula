using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    private bool playerDetected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    public bool IsPlayerDetected()
    {
        return playerDetected;
    }
}
