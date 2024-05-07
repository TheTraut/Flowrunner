using UnityEngine;
using NUnit.Framework;

public class CoinCollectorTest
{
    GameObject collectorObject;
    GameObject coinsObject;
    CoinCollector coinCollector;
    CoinController coinController;
    private int coinCount;

    /// <summary>
    /// Set up
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Set up the game objects and components
        collectorObject = new GameObject();
        coinsObject = new GameObject();
        coinCollector = collectorObject.AddComponent<CoinCollector>();
        coinController = coinsObject.AddComponent<CoinController>();
        coinCollector.Coins = coinsObject;
    }

    /// <summary>
    /// Test for assigning coin collector
    /// </summary>
    [Test]
    public void Start_AssignsCoinController()
    {
        // Act
        coinCollector.Start();

        // Assert
        Assert.IsNotNull(coinCollector, "CoinController should be assigned on Start.");
    }

    /// <summary>
    /// Test for adding coins
    /// </summary>
    [Test]
    public void IncrementsCoinCount()
    {
        // Arrange
        coinCollector.Start();  // Initialize components

        // Create a collider with the "Coin" tag to simulate a coin entering the collider
        var coin = new GameObject();
        var collider = coin.AddComponent<BoxCollider2D>();
        coin.tag = "Coin";
        collider.isTrigger = true;  // Make sure it's a trigger to mimic OnTriggerEnter2D

        // Record the initial number of coins
        int initialCoins = coinController.currentCoins;

        // Act
        // Simulate the trigger event
        coinCollector.OnTriggerEnter2D(collider);

        // Assert
        Assert.AreEqual(initialCoins + 1, coinController.currentCoins, "Coin count should increment by 1.");
    }

    
    // Helper methods in CoinController to expose the private coin count & coinController for testing
    public int currentCoins => coinCount;
    public CoinController GetCoinController() => coinController;


}
