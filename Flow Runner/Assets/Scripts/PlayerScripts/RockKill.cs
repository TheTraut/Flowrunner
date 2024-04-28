using UnityEngine;
using UnityEngine.SceneManagement;

public class RockKill : MonoBehaviour
{
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script attached to the player
    public float shieldTime = 2f; // Duration of the shield

    public GameObject Coins;
    private CoinController coinController;

    /// <summary>
    /// Handles collisions with rocks and player's shield.
    /// </summary>
    private void Start()
    {
        // Find the player GameObject by tag
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
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
    /// Updates the state of the player's shield.
    /// </summary>
    public void Update()
    {
        coinController = Coins.GetComponent<CoinController>();
    }

    /// <summary>
    /// Handles collision events when other colliders enter the trigger.
    /// </summary>
    /// <param name="other">The Collider2D object that has entered the trigger.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!PauseManager.isPaused && other.CompareTag("Player")) // Check if the game is not paused and collides with player
        {
            if (!playerMovement.shielded) // Check if the player is not shielded
            {
                // Handle player death here (e.g., restart the level, reduce player health, etc.)
                //Debug.Log("Player hit by rock!");
                SceneManager.LoadSceneAsync("Title Screen"); // Load the title screen

                // Destroy the rock after hitting the player
                Destroy(gameObject); // Destroy the rock GameObject
            }
            else
            {
                Destroy(gameObject); // Destroy the rock GameObject even if the player is shielded
            }
        }
        else if (!PauseManager.isPaused && other.CompareTag("Ground")) // Check if the game is not paused and collides with ground
        {
            Destroy(gameObject); // Destroy the rock GameObject when colliding with ground
        }
    }
}