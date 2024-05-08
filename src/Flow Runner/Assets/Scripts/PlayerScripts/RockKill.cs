using UnityEngine;
using UnityEngine.SceneManagement;

public class RockKill : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script attached to the player
    public float shieldTime = 2f; // Duration of the shield

<<<<<<< Updated upstream
=======
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script attached to the player


>>>>>>> Stashed changes
    /// <summary>
    /// Handles collisions with rocks and player's shield.
    /// </summary>
    private void Start()
    {
<<<<<<< Updated upstream
        // Find the player GameObject by tag
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
=======
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
>>>>>>> Stashed changes
        {
            // Get the PlayerMovement component from the player GameObject
            playerMovement = playerGameObject.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    /// <summary>
    /// Handles collision events when other colliders enter the trigger.
    /// </summary>
    /// <param name="other">The Collider2D object that has entered the trigger.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
       
        if (!PauseManager.IsPaused && other.CompareTag("Player")) // Check if the game is not paused and collides with player
        {
            if (!playerMovement.shielded) // Check if the player is not shielded
            {
                // Handle player death here (e.g., restart the level, reduce player health, etc.)
                //Debug.Log("Player hit by rock!");
                SceneManager.LoadSceneAsync("Title Screen"); // Load the title screen

                SceneManager.LoadSceneAsync("GameOverScreen"); // Load the title screen
                
                // Destroy the rock after hitting the player
                Destroy(gameObject); // Destroy the rock GameObject
            }
            else
            {
                Destroy(gameObject); // Destroy the rock GameObject even if the player is shielded
            }
        }
        else if (!PauseManager.IsPaused && other.CompareTag("Ground")) // Check if the game is not paused and collides with ground
        {
            Destroy(gameObject); // Destroy the rock GameObject when colliding with ground
        }
    }
}