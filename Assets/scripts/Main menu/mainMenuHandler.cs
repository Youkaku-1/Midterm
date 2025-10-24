using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class mainMenuHandler : MonoBehaviour
{

    public GameObject homeMenu;
    public GameObject optionsMenu;
    public GameObject lockerMenu;
    public GameObject mute;
    public GameObject unmute;
    


    public AudioSource audio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void onQuitPressed()
    {
        StartCoroutine(QuitAfterDelay(1f)); // Start coroutine
    }

    IEnumerator QuitAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void onPlayPressed()
    {
        StartCoroutine(StartGameAfterDelay(1f));
    }

    IEnumerator StartGameAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("started");
        SceneManager.LoadScene("Level1");
    }

    public void onOptionsPress()
    {
        StartCoroutine(openOptions(0f));
    }

    IEnumerator openOptions(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        homeMenu.SetActive(false);
        optionsMenu.SetActive(true);
        lockerMenu.SetActive(false);
        Debug.Log("options pressed");
    }

    public void onReturnPress()
    {
        StartCoroutine(returnToHome(0f));
    }

    IEnumerator returnToHome(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        homeMenu.SetActive(true);
        optionsMenu.SetActive(false);
        lockerMenu.SetActive(false);
        Debug.Log("return pressed");
    }

    public void onMutePress()
    {
        StartCoroutine(muteAction(0f));
     
    }

    IEnumerator muteAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioListener.volume = 0f;
        unmute.SetActive(false);
        mute.SetActive(true);
    }

    public void onUnmutePress()
    {
        StartCoroutine(unmuteAction(0f));
    }

    IEnumerator unmuteAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioListener.volume = 1f;
        mute.SetActive(false);
        unmute.SetActive(true);
    }

    public void onLockerPress()
    {
        StartCoroutine(lockerAction(0f));

    }

    IEnumerator lockerAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        homeMenu.SetActive(false);
        optionsMenu.SetActive(false);
        lockerMenu.SetActive(true);
    }




    public void PlayAudio()
    {
        if (audio != null)
        {
            audio.Play(); // Play the attached AudioClip

        }
        else
        {
            Debug.LogWarning("No AudioSource component found on this GameObject!");
        }
    }
}
