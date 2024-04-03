using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab;
    public float minSpawnInterval = 3f; // Minimum interval between bird spawns
    public float maxSpawnInterval = 8f; // Maximum interval between bird spawns
    public Vector2 spawnPositionRangeX = new Vector2(-10f, 10f);
    public Vector2 spawnPositionRangeY = new Vector2(0f, 5f);
    public float despawnTime = 20f; // Time after which the bird will be despawned
    public int maxBirds = 3; // Maximum number of birds at a time

    private float timeSinceLastSpawn;
    private float currentSpawnInterval;
    private int currentBirdCount;

    void Start()
    {
        timeSinceLastSpawn = 0f;
        SetRandomSpawnInterval();
        currentBirdCount = 0;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= currentSpawnInterval && currentBirdCount < maxBirds)
        {
            SpawnBird();
            timeSinceLastSpawn = 0f;
            SetRandomSpawnInterval();
        }
    }

    void SpawnBird()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnPositionRangeX.x, spawnPositionRangeX.y),
            Random.Range(spawnPositionRangeY.x, spawnPositionRangeY.y),
            transform.position.z
        );
        GameObject bird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
        currentBirdCount++;
        Destroy(bird, despawnTime); // Despawn the bird after despawnTime seconds
        // Decrement bird count when the bird is destroyed
        StartCoroutine(DecrementBirdCountAfterDelay(despawnTime));
    }

    void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    IEnumerator DecrementBirdCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentBirdCount--;
    }
}
