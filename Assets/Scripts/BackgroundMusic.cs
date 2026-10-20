
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip music;
    [Range(0f, 1f)]
    public float volume = 0.5f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = music;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (music != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Please assign a music clip!");
        }
    }
}
