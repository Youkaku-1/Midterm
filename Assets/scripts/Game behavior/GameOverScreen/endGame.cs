using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameUI;
    public TextMeshProUGUI Points;


    private void Start()
    {
        
    }

    public void Setup(int score)
    {
        gameUI.SetActive(false);
        gameObject.SetActive(true);
        Points.text = "Score: "+score.ToString();

    }

    public void restart()
    {
        Debug.Log("restarted");
        SceneManager.LoadScene("Level1");

    }

    public void mainMenu()
    {
        Debug.Log("Switched scene");
        SceneManager.LoadScene("Main menu");

    }
}
