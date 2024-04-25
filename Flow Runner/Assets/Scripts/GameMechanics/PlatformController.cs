using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;
    private float timeUntilObstacleSpawn;
    private float speedMultiplicity;

    /// <summary>
    /// Controls the spawning and movement of obstacles on platforms.
    /// </summary>
    void Start()
    {
        StartSpawnObstacle();
        speedMultiplicity = 0.5f;
    }

    /// <summary>
    /// Updates obstacle spawning and movement.
    /// </summary>
    void Update()
    {
        // Spawn new obstacles ahead of the player
        if (!PauseManager.isPaused)
        {
            SpawnLoop();
        }
        // Despawn obstacles behind the player
        // DespawnObstacles();
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
    /// Spawns a new obstacle if the spawn time condition is met.
    /// </summary>
    void SpawnObstacle()
    {
        // Select a random obstacle prefab (excluding the first one)
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(1, obstaclePrefabs.Length)];

        // Instantiate the selected obstacle prefab at the spawn position
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

        if (spawnedObstacle.transform.position.x <= -30)
        {
            Destroy(spawnedObstacle);
        }
    }

    /// <summary>
    /// Instantiates the first obstacle at the start of the game.
    /// </summary>
    void StartSpawnObstacle()
    {
        // Select a random obstacle prefab (excluding the first one)
        GameObject obstacleToSpawn = obstaclePrefabs[0];

        // Instantiate the selected obstacle prefab at the spawn position
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;
    }
}
