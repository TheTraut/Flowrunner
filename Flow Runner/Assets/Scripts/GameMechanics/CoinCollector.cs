using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] CoinController coinController;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinController.addCoin();
        }
    }
}