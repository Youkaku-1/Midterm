using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    public void loadLevel1()
    {
        Debug.Log("loadScene1");
        SceneManager.LoadScene("Level1");

    }

    public void loadLevel2()
    {
        Debug.Log("loadScene2");
        SceneManager.LoadScene("Level2");

    }

    public void loadMainMenu()
    {
        Debug.Log("Switched to mainmenu");
        SceneManager.LoadScene("Main menu");

    }


}
