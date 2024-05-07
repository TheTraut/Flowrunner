using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class RockFallingTest
{
    private GameObject birdObject;
    private RockFalling rockFalling;
    private GameObject rockPrefab;

    [SetUp]
    public void SetUp()
    {
        // Create the bird GameObject with the RockFalling script
        birdObject = new GameObject("Bird");
        rockFalling = birdObject.AddComponent<RockFalling>();

        // Setup the rock prefab
        rockPrefab = new GameObject("Rock");
        rockPrefab.tag = "Rock";
        rockFalling.rockPrefab = rockPrefab;

        // Set the Transform for spawning
        rockFalling.birdTransform = birdObject.transform;
    }

    [TearDown]
    public void TearDown()
    {
        // Destroy objects after each test to prevent test contamination
        Object.DestroyImmediate(birdObject);
        Object.DestroyImmediate(rockPrefab);
    }

    /// <summary>
    /// Verifies that initial settings are correctly applied.
    /// </summary>
    [Test]
    public void Start_InitializesCorrectly()
    {
        rockFalling.Start();

        Assert.AreEqual(0f, rockFalling.timeSinceLastSpawn);
        Assert.AreEqual(0, rockFalling.rocksDropped);
        Assert.AreEqual(rockFalling.initialMinSpawnInterval, rockFalling.currentMinSpawnInterval);
        Assert.AreEqual(rockFalling.initialMaxSpawnInterval, rockFalling.currentMaxSpawnInterval);
    }

    /// <summary>
    /// Verifies that rocks spawn and despawn logic functions correctly.
    /// Move to play mode
    /// </summary>
    [UnityTest]
    public IEnumerator Update_SpawnsRocksAndDespawnCorrectly()
    {
        rockFalling.Start();
        rockFalling.initialMinSpawnInterval = 0.1f;
        rockFalling.initialMaxSpawnInterval = 0.1f;
        rockFalling.despawnDistance = 10f; // Setting a manageable despawn distance

        // Force an update to reduce the interval and spawn a rock
        yield return new WaitForSeconds(0.1f);
        rockFalling.Update();

        Assert.AreEqual(1, GameObject.FindGameObjectsWithTag("Rock").Length, "A rock should be spawned.");

        // Move the spawned rock out of the despawn distance and run update again
        GameObject spawnedRock = GameObject.FindWithTag("Rock");
        spawnedRock.transform.position = new Vector3(0, 0, 11); // Beyond despawn distance
        rockFalling.Update();

        yield return new WaitForSeconds(0.1f); // Allow time for despawn to occur

        Assert.AreEqual(0, GameObject.FindGameObjectsWithTag("Rock").Length, "The rock should be despawned.");
    }

    /// <summary>
    /// Tests for max rocks to be reached and despawn bird
    /// Move to play mode
    /// </summary>
    [UnityTest]
    public IEnumerator Update_WithMaxRocksDropped_DespawnsBird()
    {
        rockFalling.maxRocksDropped = 1; // Set to spawn only one rock
        rockFalling.initialMinSpawnInterval = 0.1f;
        rockFalling.initialMaxSpawnInterval = 0.1f;

        rockFalling.Start();
        yield return new WaitForSeconds(0.1f); // Wait for one spawn interval

        rockFalling.Update(); // This should spawn the rock and increase the count

        Assert.IsTrue(rockFalling == null, "The bird should be destroyed after dropping the maximum number of rocks.");
    }

    /// <summary>
    /// Verifys rock is spawned at bird location
    /// </summary>
    [Test]
    public void SpawnRock_CreatesRockAtBirdPosition()
    {
        rockFalling.SpawnRock();
        Vector3 pos = new Vector3(100, 100, 100);

        var rock = GameObject.FindWithTag("Rock");
        Assert.IsNotNull(rock, "Rock should be instantiated.");
        Assert.AreEqual(pos, rock.transform.position, "Rock should spawn at the bird's position.");
    }

    /// <summary>
    /// Test for distant rock to be despawned
    /// Move to play mode
    /// </summary>
    [Test]
    public void DespawnDistantRocks_RemovesDistantRocks()
    {
        // Setup a rock far from the bird
        var distantRock = Object.Instantiate(rockPrefab, birdObject.transform.position + Vector3.one * 100f, Quaternion.identity);

        rockFalling.DespawnDistantRocks(); // Should despawn the distant rock

        Assert.IsTrue(distantRock == null, "Distant rock should be destroyed.");
    }

    /// <summary>
    /// Ensures the spawn interval is set within the specified range.
    /// </summary>
    [Test]
    public void SetRandomSpawnInterval_SetsIntervalWithinRange()
    {
        rockFalling.SetRandomSpawnInterval();

        Assert.IsTrue(rockFalling.currentSpawnInterval >= rockFalling.currentMinSpawnInterval &&
                      rockFalling.currentSpawnInterval <= rockFalling.currentMaxSpawnInterval);
    }
}
