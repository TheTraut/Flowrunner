using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class TotalCoinController : MonoBehaviour
{
    private string textCoin;
    Text coin;
    public int totalCoins = 0;    
    private string totalCoinsFilePath;

    /// <summary>
    /// Controls the display of the total number of coins collected.
    /// </summary>
    void Awake()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        UpdateCoins();
    }

    /// <summary>
    /// Initializes the total coin count and updates the text display.
    /// </summary>
    void Start()
    {
        UpdateCoins();
    }

    /// <summary>
    /// Rounds and converts the current coins into a string for display.
    /// </summary>
    /// <param name="currentCoins">The current number of coins.</param>
    /// <returns>A string representation of the current coins.</returns>
    public string TextCoin( float currentCoins) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentCoins).ToString();
    }

    /// <summary>
    /// Updates the total coin count and the text display.
    /// </summary>
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