using UnityEngine;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Prefabs & spawn settings")]
    [Tooltip("Assign one prefab per power-up type.")]
    public GameObject[] powerupPrefabs;   // assign different powerup prefabs in inspector

    [Header("Spawn area")]
    public float yPosition = 1f;          // fixed Y for all powerups
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;

    [Header("Spawn control")]
    public int maxPowerups = 4;
    public float spawnInterval = 2f;      // seconds between spawn attempts

    // Optional probabilities for each prefab (must be same length as powerupPrefabs)
    [Header("Optional: spawn weights")]
    public float[] prefabWeights;

    private List<GameObject> spawnedPowerups = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        if (powerupPrefabs == null || powerupPrefabs.Length == 0)
        {
            Debug.LogError("PowerupSpawner: No powerupPrefabs assigned!");
            enabled = false;
            return;
        }

        SpawnBatch();
    }

    void Update()
    {
        // Clean up destroyed or inactive entries
        spawnedPowerups.RemoveAll(item => item == null || !item.activeInHierarchy);

        // If all powerups are gone, spawn a new batch
        if (spawnedPowerups.Count == 0)
        {
            SpawnBatch();
        }

        // Timer logic for staggered spawns
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (spawnedPowerups.Count < maxPowerups)
            {
                SpawnPowerup();
            }
        }
    }

    void SpawnBatch()
    {
        for (int i = 0; i < maxPowerups; i++)
            SpawnPowerup();
    }

    void SpawnPowerup()
    {
        int prefabIndex = ChoosePrefabIndex();
        GameObject prefab = powerupPrefabs[prefabIndex];

        if (prefab == null)
        {
            Debug.LogWarning("PowerupSpawner: Prefab at index " + prefabIndex + " is null.");
            return;
        }

        // Random position
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        Vector3 pos = new Vector3(x, yPosition, z);

        // Spawn it
        GameObject powerup = Instantiate(prefab, pos, Quaternion.identity);
        spawnedPowerups.Add(powerup);
    }

    int ChoosePrefabIndex()
    {
        if (prefabWeights != null && prefabWeights.Length == powerupPrefabs.Length)
        {
            float total = 0f;
            for (int i = 0; i < prefabWeights.Length; i++)
                total += Mathf.Max(0f, prefabWeights[i]);

            if (total > 0f)
            {
                float r = Random.Range(0f, total);
                float accum = 0f;
                for (int i = 0; i < prefabWeights.Length; i++)
                {
                    accum += Mathf.Max(0f, prefabWeights[i]);
                    if (r <= accum) return i;
                }
            }
        }

        // fallback: uniform random
        return Random.Range(0, powerupPrefabs.Length);
    }
}
