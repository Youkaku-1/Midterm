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
    // Start is called before the first frame update.
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
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
            ParticleSystem PS  = other.gameObject.GetComponent<ParticleSystem>();
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

