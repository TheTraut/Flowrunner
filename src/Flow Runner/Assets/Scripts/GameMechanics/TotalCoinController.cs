using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TotalCoinController : MonoBehaviour
{
    private string textCoin;
    public Text coin;
    public int totalCoins = 0;
    public string totalCoinsFilePath;

    private readonly string totalCoinsFileName = "totalCoins.json";

    /// <summary>
    /// Controls the display of the total number of coins collected.
    /// </summary>
    public void Awake()
    {
        totalCoinsFilePath = Path.Combine(Application.persistentDataPath, totalCoinsFileName);
        Load();
    }

    public void Load()
    {
        totalCoinsFilePath = Path.Combine(Application.persistentDataPath, totalCoinsFileName);
        if (File.Exists(totalCoinsFilePath))
        {
            string jsonData = File.ReadAllText(totalCoinsFilePath);
            CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
            totalCoins = data.totalCoins;
        }
        else
        {
            Debug.LogWarning("Total coins file not found. Creating new total coins file.");
            totalCoins = 0;

            // Create and save new total coins file
            Save();
        }
    }

    /// <summary>
    /// Initializes the total coin count and updates the text display.
    /// </summary>
    public void Start()
    {
        UpdateCoins();
    }

    /// <summary>
    /// Rounds and converts the current coins into a string for display.
    /// </summary>
    /// <param name="currentCoins">The current number of coins.</param>
    /// <returns>A string representation of the current coins.</returns>
    public string TextCoin(float currentCoins) // rounds and converts score into a string to be displayed 
    {
        return Mathf.RoundToInt(currentCoins).ToString();
    }

    /// <summary>
    /// Updates the total coin count and the text display.
    /// </summary>
    public void UpdateCoins()
    {
        string jsonData = File.ReadAllText(totalCoinsFilePath);
        CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
        totalCoins = data.totalCoins;
        coin = GetComponent<Text>();
        textCoin = TextCoin(totalCoins);
        coin.text = textCoin;
    }

    /// <summary>
    /// Saves current total coins to a file.
    /// </summary>
    public void Save()
    {
        CoinData data = new(totalCoins);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(totalCoinsFilePath, jsonData);
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

    // Default constructor
    public CoinData()
    {
        totalCoins = 0;
    }
}