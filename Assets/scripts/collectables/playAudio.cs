using UnityEngine;

public class playAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Collider soundTrigger;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        soundTrigger = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (soundTrigger != null)
        {
            audioSource.Play();
        }
    }
}
