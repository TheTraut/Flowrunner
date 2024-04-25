using UnityEngine;
using UnityEngine.SceneManagement;

public class RockCollision : MonoBehaviour
{
    public GameObject Player;
    public PlayerMovement script;
    public bool isShielded;
    public float shieldTime = 2f;

    public void Start()
    {
        isShielded = false;
    }
    public void Update()
    {
        CheckShield();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            if (!isShielded)
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
        
    }
    void CheckShield()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isShielded = true;
            Invoke("NoShield", shieldTime);
        }
    }

    void NoShield()
    {
        isShielded = false;
    }

}