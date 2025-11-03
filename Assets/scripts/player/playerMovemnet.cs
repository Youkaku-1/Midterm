using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // ===== AUDIO =====
    [Header("Audio")]
    private AudioSource audioSource;

    // ===== POWER-UP SYSTEM =====
    [Header("Power-up References")]
    private CountdownTimer timerScript;
    private ObstacleSpawner obstacleSpawner;

    [Header("Power-up Settings")]
    public float speedBoost = 3f;
    public int speedBoostMultiplyer = 2;
    public float timeStop = 3f;
    public float disableObstacleSpawn = 3f;

    // Start is called before the first frame update.
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

        // Get references to other scripts
        timerScript = FindObjectOfType<CountdownTimer>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();

        // Start the coin type changing routine
        coinTypeCoroutine = StartCoroutine(ChangeCoinTypeRoutine());

        // Initialize UI
        UpdateCoinTypeUI();
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();
        // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // FixedUpdate is called once per fixed frame-rate frame.
    private void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin"))
        {
            ParticleSystem PS = other.gameObject.GetComponent<ParticleSystem>();
            Renderer rend = other.gameObject.GetComponent<Renderer>();

            if (rend != null)
            {
                Color coinColor = rend.material.color;

                // Check if collected coin matches the current required type
                if (ColorsAreSimilar(coinColor, currentCoinType))
                {
                    // Correct coin type - increase coins
                    coin++;
                }
                else
                {
                    // Wrong coin type - decrease coins
                    coin--;
                }
            }
            else
            {
                Debug.LogWarning("Coin object has no Renderer to get colour from!");
            }

            if (PS != null)
            {
                PS.Play();
                Debug.Log("played particles");
            }

            // Update UI and play sound & disable coin
            coinText.text = " Coin: " + coin.ToString();
            other.gameObject.SetActive(false);
            audioSource.Play();
        }
        else if (other.gameObject.CompareTag("powerUp"))
        {
            HandlePowerUp(other);
        }
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
            // Wait for random time between changes
            float waitTime = Random.Range(minTimeBetweenChanges, maxTimeBetweenChanges);
            yield return new WaitForSeconds(waitTime);

            // Toggle between yellow and red
            if (ColorsAreSimilar(currentCoinType, Color.yellow))
            {
                currentCoinType = Color.red;
            }
            else
            {
                currentCoinType = Color.yellow;
            }

            // Update UI
            UpdateCoinTypeUI();

            Debug.Log("Coin type changed to: " + (ColorsAreSimilar(currentCoinType, Color.yellow) ? "YELLOW" : "RED"));
        }
    }

    void UpdateCoinTypeUI()
    {
        if (coinTypeText != null)
        {
            // Update text and color based on current coin type
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

    void HandlePowerUp(Collider powerUp)
    {
        Renderer rend = powerUp.GetComponent<Renderer>();
        if (rend != null)
        {
            Color col = rend.material.color;

            // Green - Speed boost
            if (ColorsAreSimilar(col, Color.green))
            {
                StartCoroutine(SpeedBoost());
                Debug.Log("Speed boost activated!");
            }
            // Pink - Time stop
            else if (ColorsAreSimilar(col, new Color(1f, 0.41f, 0.71f))) // Pink
            {
                StartCoroutine(TimeStop());
                Debug.Log("Time stop activated!");
            }
            // Blue - Stop obstacles
            else if (ColorsAreSimilar(col, Color.blue))
            {
                StartCoroutine(StopObstacles());
                Debug.Log("Obstacle stop activated!");
            }
        }

        // Play particles if available
        ParticleSystem PS = powerUp.GetComponent<ParticleSystem>();
        if (PS != null)
        {
            PS.Play();
        }

        // Disable the powerup
        powerUp.gameObject.SetActive(false);
        audioSource.Play();
    }

    IEnumerator SpeedBoost()
    {
        float originalSpeed = speed;
        speed *= speedBoostMultiplyer; // Double the speed

        yield return new WaitForSeconds(speedBoost); // Boost lasts 3 seconds

        speed = originalSpeed; // Return to normal speed
    }

    IEnumerator TimeStop()
    {
        if (timerScript != null)
        {
            timerScript.timerIsRunning = false; // Stop the timer

            yield return new WaitForSeconds(timeStop); // Time stopped for 2 seconds

            timerScript.timerIsRunning = true; // Resume the timer
        }
    }

    IEnumerator StopObstacles()
    {
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false; // Disable obstacle spawning

            yield return new WaitForSeconds(disableObstacleSpawn); // Stop obstacles for 4 seconds

            obstacleSpawner.enabled = true; // Enable obstacle spawning again
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Decrease coin count by 1
            coin--;

            // Update UI
            coinText.text = " Coin: " + coin.ToString();

            Debug.Log("Hit by enemy! Coins: " + coin);
        }
    }

    // Clean up coroutine when object is destroyed
    void OnDestroy()
    {
        if (coinTypeCoroutine != null)
        {
            StopCoroutine(coinTypeCoroutine);
        }
    }
}