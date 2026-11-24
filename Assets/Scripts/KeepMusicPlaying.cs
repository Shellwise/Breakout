using UnityEngine;

// Put this on your MusicManager object.
// Keeps the music playing when Retry reloads the scene, instead of starting it again.
public class KeepMusicPlaying : MonoBehaviour
{
    static KeepMusicPlaying instance;

    void Awake()
    {
        // After a reload, the scene makes a new MusicManager. Get rid of it
        // so the original one keeps playing.
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}