using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab; // Assign your bird prefab in the Inspector
    public float spawnInterval = 5f; // Time between each spawn
    public Vector2 spawnPositionRange = new Vector2(0f, 10f); // Range for random spawn positions

    private float timeSinceLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn new birds
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnBirds();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnBirds()
    {
        int numberOfBirds = Random.Range(1, 4); // Randomly choose between 1 and 3 birds

        for (int i = 0; i < numberOfBirds; i++)
        {
            // Generate a random spawn position within the specified range
            Vector3 spawnPosition = new Vector3(Random.Range(spawnPositionRange.x, spawnPositionRange.y), transform.position.y, transform.position.z);

            // Instantiate the bird prefab at the random spawn position
            Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
