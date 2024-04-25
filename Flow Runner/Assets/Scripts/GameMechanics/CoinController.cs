using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class CoinController : MonoBehaviour
{
    public int currentCoins = 0;
    public int totalCoins = 0;
    private string textCoin;
    private string totalCoinsFilePath;
    Text coin;

    void Awake()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        Debug.Log("File path set to: " + totalCoinsFilePath);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadCoins();
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
    public void SaveCoins()
    {
        totalCoinsFilePath = Application.persistentDataPath + "/totalCoins.json";
        if (string.IsNullOrEmpty(totalCoinsFilePath))
        {
            Debug.LogError("Save path is null or empty!");
            return;
        }

        CoinData data = new CoinData(totalCoins);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(totalCoinsFilePath, jsonData);
    }

    void LoadCoins()
    {
        Debug.Log("Loading coins from: " + totalCoinsFilePath);
        if (!File.Exists(totalCoinsFilePath))
        {
            Debug.LogWarning("File does not exist. Initializing coins to zero.");
            return;
        }

        string jsonData = File.ReadAllText(totalCoinsFilePath);
        CoinData data = JsonUtility.FromJson<CoinData>(jsonData);
        totalCoins = data.totalCoins;
    }

    public void UpdateCoins()
    {
        totalCoins ++;
        Debug.Log("Total Coins" + totalCoins);
        SaveCoins();
    }
    public void addCoin()
    {
        currentCoins++;
        UpdateCoins();
    }
}
