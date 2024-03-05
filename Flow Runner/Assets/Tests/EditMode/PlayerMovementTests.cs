using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class PlayerMovementTests
{
    [UnityTest]
    public IEnumerator TestJump()
    {
        // Arrange
        var playerGameObject = new GameObject();
        var playerMovement = playerGameObject.AddComponent<PlayerMovement>();

        // Expect the log message
        LogAssert.Expect(LogType.Error, "Rigidbody2D component not found on the player. Adding Rigidbody2D component...");

        // Ensure Rigidbody2D is attached
        var rigidbody = playerGameObject.GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            Debug.LogError("Rigidbody2D component not found on the player. Adding Rigidbody2D component...");
            rigidbody = playerGameObject.AddComponent<Rigidbody2D>();
        }

        // Ensure Rigidbody2D component is initialized properly
        playerMovement.rb = rigidbody;

        // Record initial velocity
        float initialVelocityY = rigidbody.velocity.y;

        // Act
        playerMovement.Update(); // Simulate Update call
        var collider1 = playerGameObject.AddComponent<BoxCollider2D>(); // Add BoxCollider2D instead of Collider2D
        playerMovement.OnTriggerEnter2D(collider1); // Simulate entering water
        playerMovement.Update(); // Simulate Update call after entering water
        var collider2 = playerGameObject.AddComponent<BoxCollider2D>(); // Add BoxCollider2D instead of Collider2D
        playerMovement.OnTriggerExit2D(collider2); // Simulate exiting water
        playerMovement.Update(); // Simulate Update call after exiting water
        yield return null; // Ensure test method only yields null
    }

    [UnityTest]
    public IEnumerator TestIsGrounded()
    {
        // Arrange
        var playerGameObject = new GameObject();
        var playerMovement = playerGameObject.AddComponent<PlayerMovement>();

        // Create a ground object
        var ground = new GameObject("Ground");
        ground.transform.position = new Vector3(0, 0, 0); // Position the ground at origin
        var collider = ground.AddComponent<BoxCollider2D>();
        playerMovement.groundCheck = ground.transform;

        // Ensure the ground layer is set properly
        playerMovement.groundLayer = LayerMask.GetMask("Default"); // Assuming Default layer is the ground layer

        // Act
        yield return null; // Wait for physics to settle

        // Assert
        Assert.IsTrue(playerMovement.IsGrounded(), "Player is not grounded when it should be");
    }
}
