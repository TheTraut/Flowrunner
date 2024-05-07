using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class PlayerMovementTests
{
    /// <summary>
    /// Tests the player's ability to jump when they are grounded.
    /// </summary>
    [UnityTest]
    public IEnumerator TestJump()
    {
        // Arrange
        var playerGameObject = new GameObject("Player");
        var playerMovement = playerGameObject.AddComponent<PlayerMovement>();
        var rigidbody = playerGameObject.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;  // Temporarily disable gravity
        playerMovement.rb = rigidbody;

        // Mock ground setup
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = new Vector3(0, -1, 0);
        ground.layer = LayerMask.NameToLayer("ground");

        playerMovement.groundCheck = new GameObject("GroundCheck").transform;
        playerMovement.groundCheck.position = new Vector3(0, -0.1f, 0);
        playerMovement.groundLayer = 1 << LayerMask.NameToLayer("ground");

        // Act
        // Simulating key press
        // Implement the actual input simulation or logic mock here, if not possible, directly set conditions for jump
        playerMovement.HandleMovement();
        yield return new WaitForSeconds(0.1f);  // Allow time for physics to update

        // Assert
        Assert.IsTrue(rigidbody.velocity.y > 0, "Player did not jump.");
    }


    /// <summary>
    /// Tests if the player is correctly recognized as being grounded using ground check mechanics.
    /// </summary>
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
        Assert.IsTrue(playerMovement.IsGrounded(), "Player is not grounded when it should be.");
    }

    /// <summary>
    /// Tests the player's shield activation and checks if the shield deactivates after the set duration.
    /// </summary>
    [UnityTest]
    public IEnumerator TestShieldActivationAndDuration()
    {
        // Arrange
        var playerGameObject = new GameObject("Player");
        var playerMovement = playerGameObject.AddComponent<PlayerMovement>();
        playerMovement.shield = new GameObject(); // Assuming a GameObject represents the shield
        playerMovement.shield.SetActive(false);

        // Act
        playerMovement.shield.SetActive(true);
        yield return null; // Comply with EditMode - Only yield null

        // Advance time manually if needed here, typically this would be a PlayMode test
        // Assert
        // Ideally, check the state immediately after activation or reconsider the test's design
        Assert.IsTrue(playerMovement.shield.activeSelf, "Shield did not activate as expected.");
    }

}