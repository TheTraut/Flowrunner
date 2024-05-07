using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class CoinController : MonoBehaviour
{
    public int currentCoins = 0;
    public int totalCoins = 0;
    private string textCoin;
    public string totalCoinsFilePath;
    public Text coin;

    /// <summary>
    /// Manages the collection and display of coins in the game.
    /// </summary
    public void Awake()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        //Debug.Log("File path set to: " + totalCoinsFilePath);
    }

    /// <summary>
    /// Loads initial coin data and sets up text display.
    /// </summary>
    public void Start()
    {
        LoadCoins();
        coin = GetComponent<Text>();
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
    /// Updates the text display of the current coins.
    /// </summary>
    public void Update()
    {
        textCoin = TextCoin(currentCoins);
        coin.text = textCoin;
    }

    /// <summary>
    /// Saves the total coins to a JSON file.
    /// </summary>
    public void SaveCoins()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        if (string.IsNullOrEmpty(totalCoinsFilePath))
        {
            Debug.LogError("totalCoins.json save path is null or empty.");
            return;
        }

        CoinData data = new(totalCoins);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(totalCoinsFilePath, jsonData);
    }

    /// <summary>
    /// Loads the total coins from a JSON file.
    /// </summary>
    public void LoadCoins()
    {
        //Debug.Log("Loading coins from: " + totalCoinsFilePath);
        if (!File.Exists(totalCoinsFilePath))
        {
            Debug.LogWarning("File does not exist. Initializing coins to zero.");
            return;
        }

        string jsonData = File.ReadAllText(totalCoinsFilePath);
        CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
        totalCoins = data.totalCoins;
    }

    /// <summary>
    /// Updates the total coins and saves them.
    /// </summary>
    public void UpdateCoins()
    {
        totalCoins++;
        //Debug.Log("Total Coins" + totalCoins);
        SaveCoins();
    }

    /// <summary>
    /// Adds a coin to the current count and updates total coins.
    /// </summary>
    public void AddCoin()
    {
        currentCoins++;
        UpdateCoins();
    }
    
}