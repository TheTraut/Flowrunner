using NUnit.Framework;
using UnityEngine;
using System.IO;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Collections;

public class TotalCoinControllerTest
{
    private GameObject gameObject;
    private TotalCoinController coinController;
    private string filePath;

    /// <summary>
    /// Set up
    /// </summary>
    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();
        coinController = gameObject.AddComponent<TotalCoinController>();
        coinController.coin = gameObject.AddComponent<Text>(); // Ensure a Text component is present
        filePath = Application.persistentDataPath + "/totalCoins.json";
        File.WriteAllText(filePath, "{\"totalCoins\":10}");

        // Initialize Awake right after setting up to ensure path is set
        coinController.Awake();
    }

    /// <summary>
    /// Delete after use
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(gameObject);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Test for updating coins
    /// </summary>
    [UnityTest]
    public IEnumerator Start_UpdatesCoins()
    {
        // Act
        coinController.Start();
        yield return null; // Allow any frames to process if needed

        // Assert
        Assert.AreEqual("10", coinController.coin.text, "Coin text should be updated to '10' after Start.");
    }

    /// <summary>
    /// Test for updates to text
    /// </summary>
    [Test]
    public void UpdateCoins_UpdatesTotalCoinsAndText()
    {
        // Change file content to ensure update is reading new data
        File.WriteAllText(filePath, "{\"totalCoins\":15}");

        // Act
        coinController.UpdateCoins();

        // Assert
        Assert.AreEqual(15, coinController.totalCoins, "Total coins should be updated to '15'.");
        Assert.AreEqual("15", coinController.coin.text, "Coin text should be updated to '15'.");
    }
}
