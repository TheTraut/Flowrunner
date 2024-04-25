using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    public float currentCoins = 0f;
    private string textCoin;
    Text coin;
    // Start is called before the first frame update
    void Start()
    {
        coin = GetComponent<Text>();
    }
    public string TextCoin( float currentCoins) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentCoins).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        textCoin = TextCoin(currentCoins);
        coin.text = textCoin;
    }

    public void addCoin()
    {
        currentCoins = currentCoins + 1;
    }

}
