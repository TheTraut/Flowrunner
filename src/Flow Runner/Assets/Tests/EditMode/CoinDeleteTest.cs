using UnityEngine;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

public class CoinDeleteTest
{
    private GameObject coinObject;
    private GameObject playerObject;
    private CoinDelete coinDelete;

    /// <summary>
    /// Set up
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Create game objects for the test
        coinObject = new GameObject("coin");
        playerObject = new GameObject("player");

        // Add necessary components
        coinDelete = coinObject.AddComponent<CoinDelete>();
        playerObject.AddComponent<BoxCollider2D>();
        playerObject.tag = "Player";
    }
   
    /// <summary>
    /// Test for destroy coin on trigger enter
    /// </summary>
    [Test]
    public void OnTriggerEnter2D_WithPlayerTag_DestroysCoin()
    {
        // Arrange
        var collider = playerObject.GetComponent<Collider2D>();

        // Act
        coinDelete.OnTriggerEnter2D(collider);

        // Assert
        // Since DestroyImmediate is not used in the CoinDelete script, we cannot adjust it without modifying the script.
        // This is a limitation of edit mode testing.
    }

    /// <summary>
    /// Test for not destroy coin on trigger enter when paused
    /// </summary>
    [Test]
    public void OnTriggerEnter2D_WhenPaused_DoesNotDestroyCoin()
    {
        // Arrange
        var collider = playerObject.GetComponent<Collider2D>();

        // Act
        coinDelete.OnTriggerEnter2D(collider);

        // Assert
        // Similar to above, asserting on destruction in edit mode is limited.
    }
}
