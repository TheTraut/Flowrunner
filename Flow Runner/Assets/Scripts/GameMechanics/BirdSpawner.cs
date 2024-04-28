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
        if (!PauseManager.isPaused)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= currentSpawnInterval && currentBirdCount < maxBirds)
            {
                SpawnBird();
                timeSinceLastSpawn = 0f;
                SetRandomSpawnInterval();
            }
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
        StartCoroutine(DespawnBirdCoroutine(bird, despawnTime));
    }

    IEnumerator DespawnBirdCoroutine(GameObject bird, float delay)
    {
        float despawnTimer = delay;
        while (despawnTimer > 0f)
        {
            if (!PauseManager.isPaused)
            {
                despawnTimer -= Time.deltaTime;
            }
            yield return null;
        }
        Destroy(bird);
        currentBirdCount--;
    }

    void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}