using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelsMenu : MonoBehaviour
{
    public void onLevel1Pressed()
    {
        StartCoroutine(StartGameAfterDelay(1f, "Level1"));
    }

    public void onLevel2Pressed()
    {
        StartCoroutine(StartGameAfterDelay(1f, "Level2"));
    }

    IEnumerator StartGameAfterDelay(float delayTime, string scene)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("Loading: " + scene);
        SceneManager.LoadScene(scene);
    }
}       