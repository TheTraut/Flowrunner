using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class CoinControllerTests
{
    private CoinController coinController;
    private GameObject gameObject;

    /// <summary>
    /// Set up objects
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Setup the environment for each test
        gameObject = new GameObject();
        coinController = gameObject.AddComponent<CoinController>();
        coinController.coin = gameObject.AddComponent<Text>();
    }

    /// <summary>
    /// Tests for setting file path
    /// </summary>
    [Test]
    public void Awake_SetsTotalCoinsFilePath()
    {
        // Act
        coinController.Awake();

        // Assert
        string expectedPath = Application.persistentDataPath + "/totalCoins.json";
        Assert.AreEqual(expectedPath, coinController.totalCoinsFilePath, "Total coins file path should be correctly set in Awake.");
    }

    /// <summary>
    /// Test for loading coin text component
    /// </summary>
    [Test]
    public void Start_LoadsCoinsAndSetsTextComponent()
    {
        // Setup a mock Text component
        var textComponent = gameObject.AddComponent<Text>();
        coinController.coin = textComponent;

        // Act
        coinController.Start();

        // Assert
        Assert.IsNotNull(coinController.coin, "Text component should be assigned.");
        // Assuming LoadCoins method is already being tested or it's effects can be observed.
    }

    /// <summary>
    /// Test for updating coin text 
    /// </summary>
    [Test]
    public void Update_UpdatesTextDisplay()
    {
        // Setup
        var textComponent = gameObject.AddComponent<Text>();
        coinController.coin = textComponent;
        coinController.currentCoins = 5;
        coinController.Start();  // To ensure coin text component is assigned and initial load happens

        // Act
        coinController.Update();

        // Assert
        string expectedText = "5";  // Assuming the current coins are 5 and TextCoin returns "5"
        Assert.AreEqual(expectedText, coinController.coin.text, "Coin text should be updated in Update.");
    }

    /// <summary>
    /// Test for rounding coins
    /// </summary>
    [Test]
    public void TextCoin_ReturnsRoundedString()
    {
        // Arrange
        float coins = 10.75f;

        // Act
        var result = coinController.TextCoin(coins);

        // Assert
        Assert.AreEqual("11", result, "TextCoin should round and convert the current coins to string.");
    }

    /// <summary>
    /// Test for saving coins
    /// </summary>
    [Test]
    public void SaveCoins_CreatesOrUpdatesFile()
    {
        // Arrange
        coinController.totalCoins = 10;
        var expectedPath = Application.persistentDataPath + "/totalCoins.json";

        // Act
        coinController.SaveCoins();

        // Assert
        Assert.IsTrue(File.Exists(expectedPath), "Should save the file to the persistent data path.");
    }

    /// <summary>
    /// Test for loading coins from file
    /// </summary>
    [Test]
    public void LoadCoins_LoadsFromExistingFile()
    {
        // Arrange
        coinController.totalCoins = 10;
        
        // Act
        coinController.SaveCoins();
        coinController.LoadCoins();
        
        // Assert
        Assert.AreEqual(10, coinController.totalCoins, "Should load total coins from file.");
    }

    /// <summary>
    /// Tests for adding coins
    /// </summary>
    [Test]
    public void AddCoin_IncrementsCurrentAndTotalCoins()
    {
        // Arrange
        coinController.currentCoins = 5;
        coinController.totalCoins = 10;

        // Act
        coinController.AddCoin();

        // Assert
        Assert.AreEqual(6, coinController.currentCoins, "Current coins should be incremented.");
        Assert.AreEqual(11, coinController.totalCoins, "Total coins should be incremented.");
    }
}
