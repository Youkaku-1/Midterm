using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Transitions")]
    [Tooltip("Drag GameObjects to disable when this transition happens")]
    public GameObject[] objectsToDisable;

    [Tooltip("Drag GameObjects to enable when this transition happens")]
    public GameObject[] objectsToEnable;

    [Header("Settings")]
    public float delayTime = 0f;

    public void ExecuteTransition()
    {
        StartCoroutine(TransitionAfterDelay(delayTime));
    }

    IEnumerator TransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable specified objects
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Enable specified objects
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        Debug.Log("Menu transition executed");
    }
}