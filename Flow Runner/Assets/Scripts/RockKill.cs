using UnityEngine;
using UnityEngine.SceneManagement;


public class RockCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player death here (e.g., restart the level, reduce player health, etc.)
            Debug.Log("Player hit by rock!");
            SceneManager.LoadSceneAsync("Title Screen");

            // Destroy the rock after hitting the player
            Destroy(gameObject);
        }
    }
}