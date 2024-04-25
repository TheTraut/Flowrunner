using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class TotalCoinController : MonoBehaviour
{
    private string textCoin;
    Text coin;
    public int totalCoins = 0;    
    private string totalCoinsFilePath;

    // Start is called before the first frame update
    void Awake()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        UpdateCoins();
    }
    void Start()
    {
        UpdateCoins();
    }
    public string TextCoin( float currentCoins) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentCoins).ToString();
    }

    void UpdateCoins()
    {
        string jsonData = File.ReadAllText(totalCoinsFilePath);
        CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
        totalCoins = data.totalCoins;
        coin = GetComponent<Text>();
        textCoin = TextCoin(totalCoins);
        coin.text = textCoin;
    }
}

[System.Serializable]
public class CoinData
{
    public int totalCoins;

    public CoinData(int totalCoins)
    {
        this.totalCoins = totalCoins;
    }
}