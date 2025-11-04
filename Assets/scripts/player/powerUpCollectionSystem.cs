using System.Collections;
using UnityEngine;

public class PowerUpSystem : MonoBehaviour
{
    [Header("Power-up References")]
    public GameObject speedBoostPrefab;
    public GameObject timeStopPrefab;
    public GameObject stopObstaclesPrefab;

    [Header("Power-up Settings")]
    public float speedBoostDuration = 3f;
    public int speedBoostMultiplier = 2;
    public float timeStopDuration = 3f;
    public float disableObstacleSpawnDuration = 3f;

    private PlayerMovement playerMovement;
    private CountdownTimer timerScript;
    private ObstacleSpawner obstacleSpawner;
    private AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        timerScript = FindObjectOfType<CountdownTimer>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
    }

    public void HandlePowerUp(Collider powerUpCollider)
    {
        GameObject powerUp = powerUpCollider.gameObject;
        string powerUpName = powerUp.name;

        bool matched = false;

        if (speedBoostPrefab != null && powerUpName.StartsWith(speedBoostPrefab.name))
        {
            StartCoroutine(SpeedBoost());
            matched = true;
        }
        else if (timeStopPrefab != null && powerUpName.StartsWith(timeStopPrefab.name))
        {
            StartCoroutine(TimeStop());
            matched = true;
        }
        else if (stopObstaclesPrefab != null && powerUpName.StartsWith(stopObstaclesPrefab.name))
        {
            StartCoroutine(StopObstacles());
            matched = true;
        }

        if (!matched)
        {
            Debug.LogWarning("PowerUp not matched: " + powerUpName);
        }

        ParticleSystem PS = powerUp.GetComponent<ParticleSystem>();
        if (PS != null) PS.Play();

        powerUp.SetActive(false);
        if (audioSource != null) audioSource.Play();
    }

    IEnumerator SpeedBoost()
    {
        float originalSpeed = playerMovement.speed;
        playerMovement.speed *= speedBoostMultiplier;

        yield return new WaitForSeconds(speedBoostDuration);
        playerMovement.speed = originalSpeed;
    }

    IEnumerator TimeStop()
    {
        if (timerScript != null)
        {
            timerScript.timerIsRunning = false;
            yield return new WaitForSeconds(timeStopDuration);
            timerScript.timerIsRunning = true;
        }
    }

    IEnumerator StopObstacles()
    {
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false;
            yield return new WaitForSeconds(disableObstacleSpawnDuration);
            obstacleSpawner.enabled = true;
        }
    }
}