using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFalling : MonoBehaviour
{
    public GameObject rockPrefab; // Assign your rock prefab in the Inspector
    public Transform birdTransform; // Assign the bird's transform in the Inspector
    public float spawnInterval = 5f; // Time between each spawn
    public float despawnDistance = 20f; // Distance from the bird at which the rock will be despawned

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

        // Check if it's time to spawn a new rock
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnRock();
            timeSinceLastSpawn = 0f;
        }

        // Check for rocks that are too far away and despawn them
        DespawnDistantRocks();
    }

    void SpawnRock()
    {
        // Instantiate the rock at the bird's position
        GameObject rock = Instantiate(rockPrefab, birdTransform.position, Quaternion.identity);
        // You can add additional logic here if needed (e.g., setting the velocity of the rock)
    }

    void DespawnDistantRocks()
    {
        // Find all rocks in the scene
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock"); // Make sure your rock prefabs have the tag "Rock"
        foreach (GameObject rock in rocks)
        {
            if (Vector3.Distance(rock.transform.position, birdTransform.position) > despawnDistance)
            {
                // Despawn the rock if it's too far from the bird
                Destroy(rock);
            }
        }
    }
}
