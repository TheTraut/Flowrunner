using UnityEngine;
using UnityEngine.SceneManagement;

public class RockKill : MonoBehaviour
{
    public GameObject Player; // Reference to the player GameObject
    public PlayerMovement script; // Reference to the PlayerMovement script attached to the player
    public bool isShielded; // Flag to track if the player is shielded
    public float shieldTime = 2f; // Duration of the shield

    public GameObject Coins;
    private CoinController coinController;

    /// <summary>
    /// Handles collisions with rocks and player's shield.
    /// </summary>
    private void Start()
    {
        isShielded = false; // Initialize shielded flag to false
    }

    /// <summary>
    /// Updates the state of the player's shield.
    /// </summary>
    public void Update()
    {
        CheckShield(); // Check if the shield is active
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
            if (!isShielded) // Check if the player is not shielded
            {
                // Handle player death here (e.g., restart the level, reduce player health, etc.)
                Debug.Log("Player hit by rock!");
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

    /// <summary>
    /// Checks if the player's shield is active.
    /// </summary>
    void CheckShield()
    {
        if (Input.GetKey(KeyCode.Space)) // Check if the space key is pressed
        {
            isShielded = true; // Set shielded flag to true
            Invoke("NoShield", shieldTime); // Schedule the deactivation of the shield
        }
    }

    /// <summary>
    /// Deactivates the player's shield after a set duration.
    /// </summary>
    void NoShield()
    {
        isShielded = false; // Set shielded flag to false
    }
}