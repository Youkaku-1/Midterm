using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private CoinSystem coinSystem;
    private PowerUpSystem powerUpSystem;
    private CoinSystem coinManager;

    void Start()
    {
        coinSystem = GetComponent<CoinSystem>();
        powerUpSystem = GetComponent<PowerUpSystem>();
        coinManager = GetComponent<CoinSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("coin") && coinSystem != null)
        {
            coinSystem.HandleCoinCollection(other);
        }
        else if (other.gameObject.CompareTag("powerUp") && powerUpSystem != null)
        {
            powerUpSystem.HandlePowerUp(other);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && coinManager != null)
        {
            coinManager.coin -= coinManager.coinMult;
            coinManager.coinText.text = " Coin: " + coinManager.coin.ToString();
        }
    }
}