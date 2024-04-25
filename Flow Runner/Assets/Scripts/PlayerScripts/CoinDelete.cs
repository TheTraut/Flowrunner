using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinDelete : MonoBehaviour
{
    /// <summary>
    /// Handles the deletion of coins when the player collides with them.
    /// </summary>
    /// <param name="other">The Collider2D object the coin has collided with.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            // Destroy the rock after hitting the player
            Destroy(gameObject);
        }
    }
}
