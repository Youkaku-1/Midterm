using System.Collections;
using TMPro;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    [Header("Coin Amount")]
    public int coin = 0;
    public TextMeshProUGUI coinText;

    [Header("Coin Type Settings")]
    public float minTimeBetweenChanges = 5f;
    public float maxTimeBetweenChanges = 15f;
    public TextMeshProUGUI coinTypeText;
    public int coinMult = 10;

    private Color currentCoinType = Color.yellow;
    private Coroutine coinTypeCoroutine;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        coinTypeCoroutine = StartCoroutine(ChangeCoinTypeRoutine());
        UpdateCoinTypeUI();
    }

    public void HandleCoinCollection(Collider coinCollider)
    {
        Renderer rend = coinCollider.gameObject.GetComponent<Renderer>();
        ParticleSystem PS = coinCollider.gameObject.GetComponent<ParticleSystem>();

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

        if (PS != null) PS.Play();
        coinText.text = " Coin: " + coin.ToString();
        coinCollider.gameObject.SetActive(false);

        if (audioSource != null) audioSource.Play();
    }

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

            currentCoinType = ColorsAreSimilar(currentCoinType, Color.yellow) ? Color.red : Color.yellow;
            UpdateCoinTypeUI();
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

    void OnDestroy()
    {
        if (coinTypeCoroutine != null)
            StopCoroutine(coinTypeCoroutine);
    }
}