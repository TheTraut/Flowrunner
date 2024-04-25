using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinDelete : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            // Destroy the rock after hitting the player
            Destroy(gameObject);
        }
    }
}
