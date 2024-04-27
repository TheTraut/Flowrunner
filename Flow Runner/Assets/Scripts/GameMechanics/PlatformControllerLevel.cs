using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControllerLevel : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public GameObject coinPrefab; // Assign this in the Unity editor
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;
    private float timeUntilObstacleSpawn;
    private float speedMultiplicity;

    /// <summary>
    /// Controls the spawning and movement of obstacles and coins on platforms.
    /// </summary>
    void Start()
    {
        speedMultiplicity = 0.5f;
    }

    /// <summary>
    /// Updates obstacle spawning and despawning.
    /// </summary>
    void Update()
    {
        if (!PauseManager.isPaused)
        {
            SpawnLoop(); // Spawn new obstacles ahead of the player
            DespawnObstacles(); // Despawn obstacles behind the player
        }
    }

    /// <summary>
    /// Handles the continuous spawning of obstacles.
    /// </summary>
    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            SpawnObstacle();
            timeUntilObstacleSpawn = 0f;
            obstacleSpeed += speedMultiplicity;
        }
    }

    /// <summary>
    /// Despawns obstacles and coins that have moved behind a certain threshold.
    /// </summary>
    private void DespawnObstacles()
    {
        // Get all obstacle objects currently in the scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Ground");
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");

        // Loop through each obstacle
        foreach (GameObject obstacle in obstacles)
        {
            // If the obstacle has moved behind a certain threshold
            if (obstacle.transform.position.x <= -50)
            {
                // Destroy the obstacle
                Destroy(obstacle);
            }
        }
        // Loop through each coin
        foreach (GameObject coin in coins)
        {
            // If the obstacle has moved behind a certain threshold
            if (coin.transform.position.x <= -50)
            {
                // Destroy the obstacle
                Destroy(coin);
            }
        }
    }

    /// <summary>
    /// Spawns a new obstacle and a corresponding coin.
    /// </summary>
    void SpawnObstacle()
    {
        // Select a random obstacle prefab
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Instantiate the selected obstacle prefab at the spawn position
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        // Add Rigidbody2D to spawned obstacle for movement
        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

        // Instantiate the selected coin
        Vector2 coinPosition = new Vector2(spawnedObstacle.transform.position.x, spawnedObstacle.transform.position.y + 2.0f); // Adjust Y offset as needed
        GameObject spawnedCoin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);

        // Add Rigidbody2D to spawned coin for movement
        Rigidbody2D coinRB = spawnedCoin.GetComponent<Rigidbody2D>();
        coinRB.velocity = Vector2.left * obstacleSpeed;
    }
}
