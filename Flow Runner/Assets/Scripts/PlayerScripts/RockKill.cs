using UnityEngine;
using UnityEngine.SceneManagement;

public class RockKill : MonoBehaviour
{
    public GameObject Player;
    public GameObject Coins;
    private PlayerMovement script;
    private CoinController coinController;

    private void Start()
    {
        script = Player.GetComponent<PlayerMovement>();
        coinController = Coins.GetComponent<CoinController>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        bool isShielded = script.shielded;

        if (!PauseManager.isPaused && other.CompareTag("Player"))
        {
            //coinController.UpdateCoins();
            // Handle player death here (e.g., restart the level, reduce player health, etc.)
            Debug.Log("Player hit by rock!");
            SceneManager.LoadSceneAsync("Title Screen");
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
    private void die()
    {
        
    }
}