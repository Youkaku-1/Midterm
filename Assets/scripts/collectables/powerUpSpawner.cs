using UnityEngine;
using System.Collections.Generic;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab & spawn settings")]
    public GameObject powerUpPrefab;    // assign your PowerUp prefab in Inspector
    public float yPosition = 1f;        // fixed Y for all power-ups
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;

    [Header("Spawn control")]
    public int maxPowerUps = 4;
    public float spawnInterval = 2f;    // seconds between spawn attempts

    private List<GameObject> spawnedPowerUps = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        // Optionally spawn an initial batch at start
        SpawnBatch();
    }

    void Update()
    {
        // Clean up null / inactive entries from list
        spawnedPowerUps.RemoveAll(item => item == null || !item.activeInHierarchy);

        // If list is empty (all powerups collected/destroyed) then spawn a new batch
        if (spawnedPowerUps.Count == 0)
        {
            SpawnBatch();
        }

        // Interval-based spawn (staggered spawn if not at max)
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (spawnedPowerUps.Count < maxPowerUps)
            {
                SpawnPowerUp();
            }
        }
    }

    void SpawnBatch()
    {
        for (int i = 0; i < maxPowerUps; i++)
        {
            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        
    }
}

