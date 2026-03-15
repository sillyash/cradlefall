using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip clip, Transform transform, float volume)
    {
        // Spawn in GameObject
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity, transform);

        // Assign the audio clip & volume
        audioSource.clip = clip;
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Destroy after clip length
        Destroy(audioSource.gameObject, clip.length);
    }
}
