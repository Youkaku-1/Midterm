using UnityEngine;
using System.Collections;

public class LockerMenu : MonoBehaviour
{
    public playerColorStorage colorData;

    private string blueHex = "#0000FF";
    private string greenHex = "#00FF00";
    private string redHex = "#FF0000";
    private float noDelay = 0f;

    public void bluePress()
    {
        StartCoroutine(blueAction(noDelay));
    }

    IEnumerator blueAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (colorData != null)
        {
            colorData.hexValue = blueHex;
            Debug.Log("ColorData hex changed to: " + blueHex);
        }
    }

    public void greenPress()
    {
        StartCoroutine(greenAction(noDelay));
    }

    IEnumerator greenAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (colorData != null)
        {
            colorData.hexValue = greenHex;
            Debug.Log("ColorData hex changed to: " + greenHex);
        }
    }

    public void redPress()
    {
        StartCoroutine(redAction(noDelay));
    }

    IEnumerator redAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (colorData != null)
        {
            colorData.hexValue = redHex;
            Debug.Log("ColorData hex changed to: " + redHex);
        }
    }
}