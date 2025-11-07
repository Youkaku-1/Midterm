using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip; // Add this variable to drag audio files

    public void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip; // Use the assigned audio clip
            audioSource.Play();
        }
        else
        {
            if (audioSource == null)
            {
                Debug.LogWarning("No AudioSource component found!");
            }
            if (audioClip == null)
            {
                Debug.LogWarning("No AudioClip assigned!");
            }
        }
    }
}