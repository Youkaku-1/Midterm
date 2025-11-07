using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class HomeMenu : MonoBehaviour
{
    public void onQuitPressed()
    {
        StartCoroutine(QuitAfterDelay(1f));
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
}