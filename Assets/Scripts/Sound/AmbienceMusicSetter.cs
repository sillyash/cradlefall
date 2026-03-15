using UnityEngine;

public class AmbienceMusicSetter : MonoBehaviour
{
    [SerializeField] private AudioClip ambienceClip;
    [Header("Volume musique d'ambiance pour cette scène")]
    [Range(0f,1f)]
    [SerializeField] private float ambienceVolume = 1f;

    void Start()
    {
        if (ambienceClip != null && MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusic(ambienceClip, true);
            // Applique le volume spécifique à cette scène
            var musicSource = MusicManager.Instance.GetComponent<AudioSource>();
            if (musicSource != null)
            {
                musicSource.volume = ambienceVolume;
            }
        }
        // ...existing code...
    }
}
