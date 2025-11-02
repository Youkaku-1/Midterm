using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab & spawn settings")]
    public GameObject obstaclePrefab;    // assign your "Obstacle" prefab in Inspector
    public float yPosition = 1f;         // fixed Y for all obstacles
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;

    [Header("Spawn control")]
    public float spawnInterval = 2f;     // seconds between spawn attempts

    private List<GameObject> spawnedObstacles = new List<GameObject>();
    private float timer = 0f;

    void Update()
    {
        // Clean up null (destroyed) entries from list
        spawnedObstacles.RemoveAll(item => item == null);

        // Spawn obstacles continuously at the specified interval
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        // Determine random position
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        Vector3 pos = new Vector3(x, yPosition, z);

        // Instantiate the prefab
        GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);

        // Set completely random color
        Renderer rend = obstacle.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(rend.material);
            rend.material.color = new Color(Random.value, Random.value, Random.value);
        }
        else
        {
            Debug.LogWarning("Obstacle prefab has no Renderer component to set color!");
        }

        // Add to list for cleanup tracking
        spawnedObstacles.Add(obstacle);
    }
}