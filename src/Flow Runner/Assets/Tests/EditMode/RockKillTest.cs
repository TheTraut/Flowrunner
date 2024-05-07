using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class RockKillTest
{
    private GameObject rockObject;
    private RockKill rockKill;
    private GameObject playerObject;
    private PlayerMovement playerMovement;

    [SetUp]
    public void SetUp()
    {
        rockObject = new GameObject();
        rockKill = rockObject.AddComponent<RockKill>();

        playerObject = new GameObject();
        playerObject.tag = "Player";
        playerMovement = playerObject.AddComponent<PlayerMovement>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(rockObject);
        Object.DestroyImmediate(playerObject);
    }

    /// <summary>
    /// Tests if the player object is correctly found and the PlayerMovement component is attached.
    /// </summary>
    [UnityTest]
    public IEnumerator Start_FindsPlayerAndSetsPlayerMovement()
    {
        rockKill.Start();
        yield return null;  // Yield to let any start processes finish

        Assert.IsNotNull(rockKill.playerMovement, "PlayerMovement should be set in Start.");
    }

    /// <summary>
    /// Tests if the shield state change is logged.
    /// </summary>
    [Test]
    public void Update_LogsShieldStateChange()
    {
        rockKill.playerMovement = playerMovement; // Manually set PlayerMovement
        playerMovement.shielded = false; // Initially not shielded

        rockKill.Update(); // Initial update should not log anything
        playerMovement.shielded = true; // Change shield state

        LogAssert.Expect(LogType.Log, "Shielded state changed: True");
        rockKill.Update(); // This should log the shield state change
    }

    /// <summary>
    /// Tests the reaction when the rock collides with an unshielded player.
    /// </summary>
    [UnityTest]
    public IEnumerator OnTriggerEnter2D_CollidesWithUnshieldedPlayer_RestartsLevel()
    {
        var collider = playerObject.AddComponent<BoxCollider2D>();
        playerMovement.shielded = false; // Player is not shielded

        rockKill.OnTriggerEnter2D(collider);
        yield return null; // Wait a frame to ensure scene loads

        LogAssert.Expect(LogType.Log, "Player hit by rock!");
        Assert.IsFalse(rockObject); // Rock should be destroyed
    }

    /// <summary>
    /// Tests the reaction when the rock collides with a shielded player.
    /// </summary>
    [UnityTest]
    public IEnumerator OnTriggerEnter2D_CollidesWithShieldedPlayer_RockDestroyed()
    {
        var collider = playerObject.AddComponent<BoxCollider2D>();
        playerMovement.shielded = true; // Player is shielded

        rockKill.OnTriggerEnter2D(collider);
        yield return null; // Wait a frame to ensure scene does not load

        Assert.IsFalse(rockObject); // Rock should be destroyed
    }

    /// <summary>
    /// Tests the reaction when the rock collides with the ground.
    /// </summary>
    [UnityTest]
    public IEnumerator OnTriggerEnter2D_CollidesWithGround_RockDestroyed()
    {
        var groundObject = new GameObject();
        var groundCollider = groundObject.AddComponent<BoxCollider2D>();
        groundObject.tag = "Ground";

        rockKill.OnTriggerEnter2D(groundCollider);
        yield return null; // Wait a frame

        Assert.IsFalse(rockObject); // Rock should be destroyed
        Object.DestroyImmediate(groundObject);
    }
}
