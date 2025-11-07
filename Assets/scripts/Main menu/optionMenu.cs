using UnityEngine;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameObject mute;
    public GameObject unmute;

    private float noDelay = 0f;

    public void onMutePress()
    {
        StartCoroutine(muteAction(noDelay));
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
        StartCoroutine(unmuteAction(noDelay));
    }

    IEnumerator unmuteAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        AudioListener.volume = 1f;
        mute.SetActive(false);
        unmute.SetActive(true);
    }
}