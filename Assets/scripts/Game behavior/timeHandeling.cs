using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class    CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeRemaining = 60f;          // starting seconds
    public bool timerIsRunning = false;

    [Header("UI")]
    public TextMeshProUGUI timeText;   // assign TextMeshProUGUI component
    public GameOverScreen gameOver;    // gameover

    [Header("Player")]
    public GameObject Player;
    
    
    // Reference to your player movement script
    public playermovemnt playerMovement;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerIsRunning = true;
        UpdateTimeText();    // show initial value
    }

    void Update()
    {
        if (!timerIsRunning) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0f) timeRemaining = 0f;
            UpdateTimeText();
        }
        else
        {
            timerIsRunning = false;
            OnTimerEnd();
        }
    }

    void UpdateTimeText()
    {
        int seconds = Mathf.FloorToInt(timeRemaining);
        timeText.text = " Time: " + seconds.ToString();
    }

    void OnTimerEnd()
    {
        int playerCoins = playerMovement.coin;
        gameOver.Setup(playerCoins);
        if (Player != null)
        {
            Destroy(Player);
        }
        audioSource.Play();
    }
}
