using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If this scene has a configured music clip, hand it off to the persistent instance.
            if (musicSource != null && musicSource.clip != null)
            {
                Instance.SetMusic(musicSource.clip, musicSource.loop);
                var instanceSource = Instance.musicSource;
                if (instanceSource != null)
                {
                    instanceSource.volume = musicSource.volume;
                }
            }

            Destroy(gameObject); // Empêche les doublons
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void SetMusic(AudioClip newClip, bool loop = true)
    {
        if (musicSource != null && newClip != null)
        {
            if (musicSource.clip != newClip)
            {
                musicSource.clip = newClip;
                musicSource.loop = loop;
                musicSource.Play();
                return;
            }

            // Same clip: still ensure playback state matches requested settings.
            musicSource.loop = loop;
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }
}
