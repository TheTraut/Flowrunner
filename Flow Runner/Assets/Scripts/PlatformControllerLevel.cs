using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControllerLevel : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public float obstacleSpawnTime = 2f;
    public float obstacleSpeed = 1f;
    private float timeUntilObstacleSpawn;
    private float speedMultiplicity;


    void Start()
    {
        speedMultiplicity = 0.5f;
    }

    void Update()
    {
        // Spawn new obstacles ahead of the player

        SpawnLoop();


        // Despawn obstacles behind the player
        // DespawnObstacles();
    }

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



    void SpawnObstacle()
    {
        // Select a random obstacle prefab (excluding the first one)
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Instantiate the selected obstacle prefab at the spawn position
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.velocity = Vector2.left * obstacleSpeed;

    }
    

}
