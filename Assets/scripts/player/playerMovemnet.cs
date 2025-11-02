using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovemnt : MonoBehaviour
{
    // Rigidbody of the player.
    private Rigidbody rb;
    // Movement along X and Y axes.
    private float movementX;
    private float movementY;

    //Coin counter
    public int coin = 0;
    public TextMeshProUGUI coinText;

    //audio
    private AudioSource audioSource;

    // Speed at which the player moves.
    public float speed = 0;

    // Powerup references
    private CountdownTimer timerScript;
    private ObstacleSpawner obstacleSpawner;

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
                Color col = rend.material.color;

                // Using approximate checks because floating point values can vary
                if (col == Color.red)
                {
                    coin--;
                }
                else if (col == Color.yellow)
                {
                    coin++;
                }
                else
                {
                    // Optional: coin has some unexpected colour
                    // You could treat it as neutral or warn
                    Debug.LogWarning("Coin colour not recognised: " + col);
                }
            }
            else
            {
                Debug.LogWarning("Coin object has no Renderer to get colour from!");
            }
            if (PS != null)
            {
                PS.Play();
                Debug.Log("played particals");
            }

            // Update UI and play sound & disable coin
            coinText.text = "Coin: " + coin.ToString();
            other.gameObject.SetActive(false);
            audioSource.Play();
        }
        else if (other.gameObject.CompareTag("powerUp"))
        {
            HandlePowerUp(other);
        }
    }

    void HandlePowerUp(Collider powerUp)
    {
        Renderer rend = powerUp.GetComponent<Renderer>();
        if (rend != null)
        {
            Color col = rend.material.color;

            // Green - Speed boost
            if (col == Color.green)
            {
                StartCoroutine(SpeedBoost());
                Debug.Log("Speed boost activated!");
            }
            // Pink - Time stop
            else if (col == new Color(1f, 0.41f, 0.71f)) // Pink
            {
                StartCoroutine(TimeStop());
                Debug.Log("Time stop activated!");
            }
            // Blue - Stop obstacles
            else if (col == Color.blue)
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
            coinText.text = "Coin: " + coin.ToString();

            Debug.Log("Hit by enemy! Coins: " + coin);
        }
    }
}