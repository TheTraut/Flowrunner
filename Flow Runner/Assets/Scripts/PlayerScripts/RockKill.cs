using UnityEngine;
using UnityEngine.SceneManagement;

public class RockCollision : MonoBehaviour
{
    public GameObject Player;
    private PlayerMovement script;
    private void Start()
    {
        script = Player.GetComponent<PlayerMovement>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        bool isShielded = script.shielded;

        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            if (isShielded)
            {

                // Handle player death here (e.g., restart the level, reduce player health, etc.)
                Debug.Log("Player hit by rock!");
                SceneManager.LoadSceneAsync("Title Screen");

                // Destroy the rock after hitting the player
                // Destroy the rock after hitting the player
                Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
        else if (!PauseManager.isPaused && other.CompareTag("Ground")) // collision with platforms
        {
            Destroy(gameObject);
        }
        else if (!PauseManager.isPaused && other.CompareTag("Shield")) // collision with platforms
        {
            Destroy(gameObject);
        }
    }
}