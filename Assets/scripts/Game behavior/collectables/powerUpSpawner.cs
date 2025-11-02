using UnityEngine;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour
{
    [Header("Prefab & spawn settings")]
    public GameObject powerupPrefab;     // assign your "Powerup" prefab in Inspector
    public float yPosition = 1f;         // fixed Y for all powerups
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;

    [Header("Spawn control")]
    public int maxPowerups = 4;
    public float spawnInterval = 2f;     // seconds between spawn attempts

    private List<GameObject> spawnedPowerups = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        // Optionally spawn initial batch at start
        SpawnBatch();
    }

    void Update()
    {
        // Clean up null (destroyed) entries from list
        spawnedPowerups.RemoveAll(item => item == null || !item.activeInHierarchy);

        // If list is empty (all powerups destroyed) then spawn a new batch
        if (spawnedPowerups.Count == 0)
        {
            SpawnBatch();
        }

        // (Optional) You can still keep the interval logic if you want staggered spawn rather than all at once.
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
        {
            SpawnPowerup();
        }
    }

    void SpawnPowerup()
    {
        // Determine random position
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        Vector3 pos = new Vector3(x, yPosition, z);

        // Instantiate the prefab
        GameObject powerup = Instantiate(powerupPrefab, pos, Quaternion.identity);

        // Set random color between green, blue and pink
        Renderer rend = powerup.GetComponent<Renderer>();
        if (rend != null)
        {
            // Choose randomly between 0, 1, 2 for three colors
            int colorChoice = Random.Range(0, 3);
            rend.material = new Material(rend.material);

            switch (colorChoice)
            {
                case 0:
                    rend.material.color = Color.green;
                    break;
                case 1:
                    rend.material.color = Color.blue;
                    break;
                case 2:
                    rend.material.color = new Color(1f, 0.41f, 0.71f); // Pink
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Powerup prefab has no Renderer component to set color!");
        }

        // Add to list
        spawnedPowerups.Add(powerup);
    }
}