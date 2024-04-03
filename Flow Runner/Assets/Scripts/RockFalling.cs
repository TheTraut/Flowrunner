using UnityEngine;

public class RockFalling : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform birdTransform;
    public float initialMinSpawnInterval = 4f;
    public float initialMaxSpawnInterval = 6f;
    public float minimumSpawnInterval = 1f;
    public float intervalReductionRate = 0.1f; // Amount by which the spawn interval range is reduced per second
    public float despawnDistance = 20f;
    public int maxRocksDropped = 5; // Maximum number of rocks the bird can drop before despawning

    private float timeSinceLastSpawn;
    private float currentMinSpawnInterval;
    private float currentMaxSpawnInterval;
    private float currentSpawnInterval;
    private int rocksDropped;

    void Start()
    {
        timeSinceLastSpawn = 0f;
        rocksDropped = 0;
        currentMinSpawnInterval = initialMinSpawnInterval;
        currentMaxSpawnInterval = initialMaxSpawnInterval;
        SetRandomSpawnInterval();
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Reduce the spawn interval range over time, but don't let it go below the minimum
        currentMinSpawnInterval = Mathf.Max(minimumSpawnInterval, currentMinSpawnInterval - intervalReductionRate * Time.deltaTime);
        currentMaxSpawnInterval = Mathf.Max(minimumSpawnInterval, currentMaxSpawnInterval - intervalReductionRate * Time.deltaTime);

        if (timeSinceLastSpawn >= currentSpawnInterval)
        {
            SpawnRock();
            timeSinceLastSpawn = 0f;
            rocksDropped++;
            SetRandomSpawnInterval();

            if (rocksDropped >= maxRocksDropped)
            {
                Destroy(gameObject); // Despawn the bird after dropping maxRocksDropped rocks
            }
        }

        DespawnDistantRocks();
    }

    void SpawnRock()
    {
        Instantiate(rockPrefab, birdTransform.position, Quaternion.identity);
    }

    void DespawnDistantRocks()
    {
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rock");
        foreach (GameObject rock in rocks)
        {
            if (Vector3.Distance(rock.transform.position, birdTransform.position) > despawnDistance)
            {
                Destroy(rock);
            }
        }
    }

    void SetRandomSpawnInterval()
    {
        currentSpawnInterval = Random.Range(currentMinSpawnInterval, currentMaxSpawnInterval);
    }
}

