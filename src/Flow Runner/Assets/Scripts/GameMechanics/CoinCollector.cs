using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public GameObject Coins;
    private CoinController coinController;

    [SerializeField] private AudioClip coinSoundClip;

    /// <summary>
    /// Handles the collection of coins by the player.
    /// </summary>
    private void Start()
    {
        coinController = Coins.GetComponent<CoinController>();
    }

    /// <summary>
    /// Triggers when another collider enters this object's collider.
    /// </summary>
    /// <param name="other">The other collider.</param>
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            SoundFXManager.instance.PlaySoundFXClip(coinSoundClip, transform);
            coinController.AddCoin();
        }
    }
}