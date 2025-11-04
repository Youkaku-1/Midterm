using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovemnt : MonoBehaviour
{
    // ===== PLAYER MOVEMENT =====
    [Header("Player Movement Settings")]
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // ===== COIN SYSTEM =====
    [Header("Coin Amount")]
    public int coin = 0;
    public TextMeshProUGUI coinText;

    [Header("Coin settings")]
    public float minTimeBetweenChanges = 5f;
    public float maxTimeBetweenChanges = 15f;
    public TextMeshProUGUI coinTypeText;
    private Color currentCoinType = Color.yellow;
    private Coroutine coinTypeCoroutine;
    public int coinMult = 10;

    // ===== AUDIO =====
    [Header("Audio")]
    private AudioSource audioSource;

    // ===== POWER-UP SYSTEM =====
    [Header("Power-up References (assign the prefab assets here)")]
    public GameObject speedBoostPrefab;
    public GameObject timeStopPrefab;
    public GameObject stopObstaclesPrefab;

    [Header("Power-up Settings")]
    public float speedBoost = 3f;
    public int speedBoostMultiplyer = 2;
    public float timeStop = 3f;
    public float disableObstacleSpawn = 3f;

    // References to other scripts
    private CountdownTimer timerScript;
    private ObstacleSpawner obstacleSpawner;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        timerScript = FindObjectOfType<CountdownTimer>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

        coinTypeCoroutine = StartCoroutine(ChangeCoinTypeRoutine());
        UpdateCoinTypeUI();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            HandleCoinTrigger(other);
        }
        else if (other.gameObject.CompareTag("powerUp"))
        {
            HandlePowerUp(other);
        }
    }

    private void HandleCoinTrigger(Collider other)
    {
        ParticleSystem PS = other.gameObject.GetComponent<ParticleSystem>();
        Renderer rend = other.gameObject.GetComponent<Renderer>();

        if (rend != null)
        {
            Color coinColor = rend.material.color;

            if (ColorsAreSimilar(coinColor, currentCoinType))
            {
                coin += coinMult;
            }
            else
            {
                coin -= coinMult;
            }
        }
        else
        {
            Debug.LogWarning("Coin object has no Renderer to get colour from!");
        }

        if (PS != null) PS.Play();

        coinText.text = " Coin: " + coin.ToString();
        other.gameObject.SetActive(false);
        if (audioSource != null) audioSource.Play();
    }

    // Simple name-based prefab matching (works with instantiated prefabs -> "PrefabName(Clone)")
    void HandlePowerUp(Collider powerUpCollider)
    {
        GameObject go = powerUpCollider.gameObject;
        string goName = go.name;

        bool matched = false;

        if (speedBoostPrefab != null && goName.StartsWith(speedBoostPrefab.name))
        {
            StartCoroutine(SpeedBoost());
            Debug.Log("Speed boost activated!");
            matched = true;
        }
        else if (timeStopPrefab != null && goName.StartsWith(timeStopPrefab.name))
        {
            StartCoroutine(TimeStop());
            Debug.Log("Time stop activated!");
            matched = true;
        }
        else if (stopObstaclesPrefab != null && goName.StartsWith(stopObstaclesPrefab.name))
        {
            StartCoroutine(StopObstacles());
            Debug.Log("Obstacle stop activated!");
            matched = true;
        }

        if (!matched)
        {
            Debug.LogWarning("PowerUp collided but did not match any assigned prefab: " + goName);
        }

        // Play particles if available
        ParticleSystem PS = go.GetComponent<ParticleSystem>();
        if (PS != null) PS.Play();

        // Disable the powerup object
        go.SetActive(false);
        if (audioSource != null) audioSource.Play();
    }

    // Helper method to compare colors (since direct == comparison can be problematic with Color)
    private bool ColorsAreSimilar(Color a, Color b, float tolerance = 0.1f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }

    IEnumerator ChangeCoinTypeRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
            yield return new WaitForSeconds(waitTime);

            if (ColorsAreSimilar(currentCoinType, Color.yellow))
            {
                currentCoinType = Color.red;
            }
            else
            {
                currentCoinType = Color.yellow;
            }

            UpdateCoinTypeUI();
            Debug.Log("Coin type changed to: " + (ColorsAreSimilar(currentCoinType, Color.yellow) ? "YELLOW" : "RED"));
        }
    }

    void UpdateCoinTypeUI()
    {
        if (coinTypeText != null)
        {
            if (ColorsAreSimilar(currentCoinType, Color.yellow))
            {
                coinTypeText.text = " Collect YELLOW Coins!";
                coinTypeText.color = Color.yellow;
            }
            else
            {
                coinTypeText.text = " Collect RED Coins!";
                coinTypeText.color = Color.red;
            }
        }
    }

    IEnumerator SpeedBoost()
    {
        float originalSpeed = speed;
        speed *= speedBoostMultiplyer;

        yield return new WaitForSeconds(speedBoost);

        speed = originalSpeed;
    }

    IEnumerator TimeStop()
    {
        if (timerScript != null)
        {
            timerScript.timerIsRunning = false;

            yield return new WaitForSeconds(timeStop);

            timerScript.timerIsRunning = true;
        }
    }

    IEnumerator StopObstacles()
    {
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false;

            yield return new WaitForSeconds(disableObstacleSpawn);

            obstacleSpawner.enabled = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            coin -= coinMult;
            coinText.text = " Coin: " + coin.ToString();

            Debug.Log("Hit by enemy! Coins: " + coin);
        }
    }

    void OnDestroy()
    {
        if (coinTypeCoroutine != null)
        {
            StopCoroutine(coinTypeCoroutine);
        }
    }
}
