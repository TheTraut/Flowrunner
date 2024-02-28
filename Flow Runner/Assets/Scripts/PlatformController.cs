using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public float spawnDistance = 10f; // Distance ahead of the player to spawn new obstacles
    public float despawnDistance = 20f; // Distance behind the player to despawn obstacles
    public float yOffset = 0f; // Y-axis offset for spawn position
    public float obstacleSpeed = 5f; // Speed of the obstacles
    public float obstacleSpawnTime = 2f; 
    private float timeUntilObstacleSpawn; 

    private Transform player; // Reference to the player's transform
    private Vector2 spawnPosition; // Position to spawn new obstacles
    private bool spawnedStartingPrefab = false; // Flag to track if the starting prefab has been spawned

    void Start()
    {
        // Find the player object by tag (you can also assign the player object directly in the Unity Editor)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPosition = player.position + new Vector3(spawnDistance, yOffset, 0f);

        // Spawn the starting prefab
        SpawnStartingPrefab();
    }

    void Update()
    {
        // Spawn new obstacles ahead of the player
        if (player.position.x > spawnPosition.x && spawnedStartingPrefab)
        {
            SpawnLoop();
        }

        // Despawn obstacles behind the player
        DespawnObstacles();
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if(timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            SpawnObstacle();
            timeUntilObstacleSpawn = 0f;
        }
    }

    void SpawnStartingPrefab()
    {
        // Spawn the first obstacle prefab
        GameObject startingPrefab = obstaclePrefabs[0];
        Instantiate(startingPrefab, spawnPosition, Quaternion.identity);

        Rigidbody2D obstacleRB = startingPrefab.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

        // Update the spawn position for the next obstacle
        spawnPosition.x += spawnDistance;
        spawnedStartingPrefab = true;
    }

    void SpawnObstacle()
    {
        // Select a random obstacle prefab (excluding the first one)
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(1, obstaclePrefabs.Length)];

        // Instantiate the selected obstacle prefab at the spawn position
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        // Calculate direction towards the player
        //Vector2 direction = (player.position - newObstacle.transform.position).normalized;

        Rigidbody2D obstacleRB = newObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

        // Apply velocity towards the player
        //newObstacle.GetComponent<Rigidbody2D>().velocity = direction * obstacleSpeed;

        // Update the spawn position for the next obstacle
        spawnPosition.x += spawnDistance;
    }

    void DespawnObstacles()
    {
        // Get all obstacle GameObjects in the scene
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Ground");

        // Loop through each obstacle
        foreach (GameObject obstacle in obstacles)
        {
            // Despawn obstacles behind the player
            if (obstacle.transform.position.x < player.position.x - despawnDistance)
            {
                Destroy(obstacle);
            }
        }
    }
}
