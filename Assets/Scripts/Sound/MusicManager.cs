using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Empêche les doublons
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.loop = true;
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
            }
        }
    }
}
