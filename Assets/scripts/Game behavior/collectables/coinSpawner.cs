using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    [Header("Prefab & spawn settings")]
    public GameObject coinPrefab;        // assign your “Coin” prefab in Inspector
    public float yPosition = 1f;         // fixed Y for all coins
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;

    [Header("Spawn control")]
    public int maxCoins = 4;
    public float spawnInterval = 2f;     // seconds between spawn attempts

    private List<GameObject> spawnedCoins = new List<GameObject>();
    private float timer = 0f;

    void Start()
    {
        // Optionally spawn initial batch at start
        SpawnBatch();
    }

    void Update()
    {
        // Clean up null (destroyed) entries from list
        spawnedCoins.RemoveAll(item => item == null || !item.activeInHierarchy);

        // If list is empty (all coins destroyed) then spawn a new batch
        if (spawnedCoins.Count == 0)
        {
            SpawnBatch();
        }

        // (Optional) You can still keep the interval logic if you want staggered spawn rather than all at once.
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (spawnedCoins.Count < maxCoins)
            {
                SpawnCoin();
            }
        }
    }

    void SpawnBatch()
    {
        for (int i = 0; i < maxCoins; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        // Determine random position
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        Vector3 pos = new Vector3(x, yPosition, z);

        // Instantiate the prefab
        GameObject coin = Instantiate(coinPrefab, pos, Quaternion.identity);

        // Set random color between yellow and red
        Renderer rend = coin.GetComponent<Renderer>();
        if (rend != null)
        {
            // Choose randomly: 0 => yellow, 1 => red
            if (Random.Range(0, 2) == 0)
            {
                rend.material = new Material(rend.material);
                rend.material.color = Color.yellow;
            }
            else
            {
                rend.material = new Material(rend.material);
                rend.material.color = Color.red;
            }
        }
        else
        {
            Debug.LogWarning("Coin prefab has no Renderer component to set color!");
        }

        // Add to list
        spawnedCoins.Add(coin);
    }
}
