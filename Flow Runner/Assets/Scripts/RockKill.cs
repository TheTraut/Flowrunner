using UnityEngine;
using UnityEngine.SceneManagement;

public class RockCollision : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            // Handle player death here (e.g., restart the level, reduce player health, etc.)
            Debug.Log("Player hit by rock!");
            SceneManager.LoadSceneAsync("Title Screen");

            // Destroy the rock after hitting the player
            Destroy(gameObject);
        }
    }
}
