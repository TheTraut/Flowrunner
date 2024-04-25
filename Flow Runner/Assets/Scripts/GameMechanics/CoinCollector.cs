using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public GameObject Coins;
    private CoinController coinController;

private void Start()
    {
        coinController = Coins.GetComponent<CoinController>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinController.addCoin();
        }
    }
}