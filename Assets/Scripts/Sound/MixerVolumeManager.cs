using UnityEngine;
using UnityEngine.Audio;

public class MixerVolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public float GetMasterVolume()
    {
        return mixer.GetFloat("MasterVolume", out float value) ? value : 0f;
    }

    public void SetMasterVolume(float level)
    {
        mixer.SetFloat("MasterVolume", DecibelToLinear(level));
    }

    public float GetSoundFXVolume()
    {
        return mixer.GetFloat("SoundFXVolume", out float value) ? value : 0f;
    }

    public void SetSoundFXVolume(float level)
    {
        mixer.SetFloat("SoundFXVolume", DecibelToLinear(level)); 
    }

    public float GetMusicVolume()
    {
        return mixer.GetFloat("MusicVolume", out float value) ? value : 0f;
    }

    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", DecibelToLinear(level));
    }

    /// <summary>
    /// Converts a decibel value to linear volume scale.
    /// </summary>
    /// <param name="decibel">The decibel value to convert.</param>
    /// <returns>The linear volume value.</returns>
    private float DecibelToLinear(float decibel)
    {
        return Mathf.Log10(decibel) * 20f;
    }
}
