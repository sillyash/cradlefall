using UnityEngine;

public class AmbienceMusicSetter : MonoBehaviour
{
    [SerializeField] private AudioClip ambienceClip;

    void Start()
    {
        if (ambienceClip != null && MusicManager.Instance != null)
        {
            MusicManager.Instance.SetMusic(ambienceClip, true);
        }
    }
}
